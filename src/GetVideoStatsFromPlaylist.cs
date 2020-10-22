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

            string playlistId = req.Query["playlistId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            playlistId = playlistId ?? data?.playlistId;

            _APIKEY = Environment.GetEnvironmentVariable("APIKEY");
        
            var stats = await GetPlayListVideos(playlistId);

            return new OkObjectResult(new JsonResult(stats));
        }


        static async Task<List<YouTubeVideoStatistics>> GetPlayListVideos(string playlistId)
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
                        var video = await GetVideoDetails(playlistItem.Snippet.ResourceId.VideoId, playlistItem.Snippet.Title);
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
                var searchRequest = youtubeService.Videos.List("statistics");
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
                        FavoriteCount = youTubeVideo.Statistics.FavoriteCount ?? 0,
                        CommentCount = youTubeVideo.Statistics.CommentCount ?? 0,
                    };
                }
            }
            return videoDetails;
        }





    }



}
