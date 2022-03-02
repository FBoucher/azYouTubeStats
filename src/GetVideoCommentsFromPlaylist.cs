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
using Google.Apis.YouTube.v3.Data;

namespace cloud5mins.Function
{
    public static class GetVideoCommentsFromPlaylist
    {
        private static IConfiguration Config { set; get; }

        private static string _APIKEY { set; get; }

        [FunctionName("GetVideoCommentsFromPlaylist")]
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

            var comments = await GetPlayListComments(playlistId);

            return new OkObjectResult(new JsonResult(comments));
        }


        static async Task<List<YouTubeComment>> GetPlayListComments(string playlistId)
        {
            var commentList = new List<YouTubeComment>();
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
                        List<YouTubeComment> comments;
                        if (playlistItem.Snippet.Title != "Private video")
                        {
                            comments = await GetCommentList(playlistItem.Snippet.ResourceId.VideoId);
                            if(comments.Count >= 1){
                                commentList.AddRange(comments);
                            }
                            else{
                                Console.WriteLine("- No Comments for Video ({0})", playlistItem.Snippet.ResourceId.VideoId);
                            }
                        }
                        else
                        {
                            commentList.Add( new YouTubeComment() { VideoId = playlistItem.Snippet.ResourceId.VideoId });
                        }

                        Console.WriteLine("- Comments From Video ({0})", playlistItem.Snippet.ResourceId.VideoId);

                    }
                    nextPageToken = searchResponse.NextPageToken;
                }
            }
            return commentList;
        }

        static async Task<List<YouTubeComment>> GetCommentList(string videoId)
        {

            var commentList = new List<YouTubeComment>();
            using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _APIKEY,
            }))
            {
                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var searchRequest = youtubeService.CommentThreads.List("replies,snippet");
                    searchRequest.VideoId = videoId;
                    searchRequest.MaxResults = 50;
                    searchRequest.PageToken = nextPageToken;
                    var searchResponse = await searchRequest.ExecuteAsync();

                    foreach (var commentItem in searchResponse.Items)
                    {
                        // YouTubeComment comment = null;
                        // if (commentItem.Snippet.Title != "Private video")
                        // {
                        //     comment. = await GetVideoDetails(commentItem.Snippet.ResourceId.VideoId, commentItem.Snippet.Title);
                        // }
                        // else
                        // {
                            YouTubeComment comment = new YouTubeComment()
                            {

                                CommentId = commentItem.Id,
                                VideoId = commentItem.Snippet.VideoId,
                                TextOriginal = commentItem.Snippet.TopLevelComment.Snippet.TextOriginal,
                                AuthorDisplayName = commentItem.Snippet.TopLevelComment.Snippet.AuthorDisplayName,
                                LikeCount = commentItem.Snippet.TopLevelComment.Snippet.LikeCount ?? 0,
                                TotalReplyCount = commentItem.Snippet.TotalReplyCount ?? 0,
                                PublishedAt = commentItem.Snippet.TopLevelComment.Snippet.PublishedAt,
                                UpdatedAt = commentItem.Snippet.TopLevelComment.Snippet.UpdatedAt,
                                IsPublic = commentItem.Snippet.IsPublic ?? true
                            };
                        // }

                        Console.WriteLine("- ({1}) {0}", comment.VideoId, comment.TextOriginal);
                        commentList.Add(comment);
                    }
                    nextPageToken = searchResponse.NextPageToken;
                }
            }
            return commentList;
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
                        Title = title.Replace(',', ' '),
                        ViewCount = youTubeVideo.Statistics.ViewCount ?? 0,
                        LikeCount = youTubeVideo.Statistics.LikeCount ?? 0,
                        DislikeCount = youTubeVideo.Statistics.DislikeCount ?? 0,
                        CommentCount = youTubeVideo.Statistics.CommentCount ?? 0
                    };
                    if (youTubeVideo.Snippet != null)
                    {
                        videoDetails.Tags = youTubeVideo.Snippet.Tags ?? null;
                        videoDetails.PublishedAt = youTubeVideo.Snippet.PublishedAt ?? String.Empty;
                        videoDetails.Language = youTubeVideo.Snippet.DefaultAudioLanguage ?? String.Empty;
                    }
                    if (youTubeVideo.ContentDetails != null)
                    {
                        videoDetails.Duration = youTubeVideo.ContentDetails.Duration ?? String.Empty;
                    }
                }
            }
            return videoDetails;
        }





    }


    public class YouTubeComment
    {
        public string CommentId { get; set; }
        public string VideoId { get; set; }
        public string TextOriginal { get; set; }
        public string AuthorDisplayName { get; set; }
        public long LikeCount { get; set; }
        public long TotalReplyCount { get; set; }
        public string PublishedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsPublic { get; set; }

    }
}
