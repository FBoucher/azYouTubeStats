namespace cloud5mins.domain
{
    public class YouTubeVideoStatistics
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public ulong ViewCount { get; set; }
        public ulong LikeCount { get; set; }
        public ulong DislikeCount { get; set; }
        public ulong FavoriteCount { get; set; }
        public ulong CommentCount { get; set; }


        // The value is specified in ISO8601 (YYYY-MM- DDThh:mm:ss.sssZ) format.
        public virtual string RecordingDate { get; set; }
        public virtual string ETag { get; set; }


        // Live Stats
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
    }
}