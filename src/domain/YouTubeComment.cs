using System.Collections.Generic;

namespace cloud5mins.domain
{
    public class YouTubeComment
    {
        public string CommentId { get; set; }
        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string TextOriginal { get; set; }
        public string AuthorDisplayName { get; set; }
        public long LikeCount { get; set; }
        public long TotalReplyCount { get; set; }
        public string PublishedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsPublic { get; set; }

    }
}