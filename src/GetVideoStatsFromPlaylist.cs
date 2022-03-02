using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using cloud5mins.domain;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace cloud5mins.Function
{
    public static class GetVideoStatsFromPlaylist
    {
        private static IConfiguration Config { set; get; }

        private static string _APIKEY { set; get; }

        [FunctionName("GetVideoStatsFromPlaylist")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            bool addComments = false;
            string playlistId = req.Query["playlistId"];
            string withComment = req.Query["withComment"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            playlistId = playlistId ?? data?.playlistId;
            withComment = withComment ?? data?.withComment ?? "false";
            
            if(withComment.ToLower() == "true"){
                addComments = true;
            }
           

            _APIKEY = Environment.GetEnvironmentVariable("APIKEY");
        
            var stats = await GetPlayListVideos(playlistId, addComments);

            return new OkObjectResult(new JsonResult(stats));
        }


        static async Task<List<YouTubeVideoStatistics>> GetPlayListVideos(string playlistId, bool addComments)
        {
            var videoList = new List<YouTubeVideoStatistics>();
            using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _APIKEY,
            }))
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var searchRequest = youtubeService.PlaylistItems.List("snippet");
                    searchRequest.PlaylistId = playlistId;
                    searchRequest.MaxResults = 50;
                    searchRequest.PageToken = nextPageToken;
                    var searchResponse = await searchRequest.ExecuteAsync();

                    foreach (var playlistItem in searchResponse.Items)
                    {
                        YouTubeVideoStatistics video = null;
                        if(playlistItem.Snippet.Title != "Private video"){
                            video = await GetVideoDetails(playlistItem.Snippet.ResourceId.VideoId, playlistItem.Snippet.Title);

                        }
                        else{
                            video = new YouTubeVideoStatistics() {Title = playlistItem.Snippet.Title, VideoId = playlistItem.Snippet.ResourceId.VideoId};
                        }
                        if(addComments){
                            List<YouTubeComment> comments = await GetVideoCommentsFromPlaylist.GetCommentList(playlistItem.Snippet.ResourceId.VideoId, playlistItem.Snippet.Title);
                            if(comments != null && comments.Count >=1 ){
                                video.Comments = comments;
                            }
                        }
                        
                        Console.WriteLine("- ({1}) {0}", video.Title, video.ViewCount);
                        videoList.Add(video);
                    }
                    nextPageToken = searchResponse.NextPageToken;
                }
            }
            return videoList;
        }


        static async Task<YouTubeVideoStatistics> GetVideoDetails(string videoId, string title)
        {
            YouTubeVideoStatistics videoDetails = null;
            using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _APIKEY,
            }))
            {
                var searchRequest = youtubeService.Videos.List("statistics,contentDetails,snippet");
                searchRequest.Id = videoId;
                var searchResponse = await searchRequest.ExecuteAsync();

                var youTubeVideo = searchResponse.Items.FirstOrDefault();
                if (youTubeVideo != null)
                {
                    videoDetails = new YouTubeVideoStatistics()
                    {
                        VideoId = videoId,
                        Title = title.Replace(',',' '),
                        ViewCount = youTubeVideo.Statistics.ViewCount ?? 0,
                        LikeCount = youTubeVideo.Statistics.LikeCount ?? 0,
                        DislikeCount = youTubeVideo.Statistics.DislikeCount ?? 0,
                        CommentCount = youTubeVideo.Statistics.CommentCount ?? 0
                    };
                    if(youTubeVideo.Snippet != null)
                    {
                        videoDetails.Tags = youTubeVideo.Snippet.Tags ?? null;
                        videoDetails.PublishedAt = youTubeVideo.Snippet.PublishedAt ?? String.Empty;
                        videoDetails.Language = youTubeVideo.Snippet.DefaultAudioLanguage ?? String.Empty;
                    }
                    if(youTubeVideo.ContentDetails != null)
                    {
                        videoDetails.Duration = youTubeVideo.ContentDetails.Duration ?? String.Empty;
                    }
                }
            }
            return videoDetails;
        }





    }



}
