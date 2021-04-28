using System.Collections.Generic;

namespace cloud5mins.domain
{
    public class YouTubeVideoStatistics
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public ulong ViewCount { get; set; }
        public ulong LikeCount { get; set; }
        public ulong DislikeCount { get; set; }
        public ulong CommentCount { get; set; }
        public string Language { get; set; }

        public virtual IList<string> Tags { get; set; }


        // The value is specified in ISO8601 (YYYY-MM- DDThh:mm:ss.sssZ) format.
        public string PublishedAt { get; set; }

        // The value is specified in ISO 8601 duration.(PT15M33S means the video is 15 minutes and 33 seconds long).
        public string Duration { get; set; }
    }
}