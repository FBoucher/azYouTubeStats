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
    }
}