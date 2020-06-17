using System;
using TheGramFeed.Domain.Models;

namespace TheGramFeed.Domain.DTO.Response
{
    public class FeedPostsResponse
    {
        public User User { get; set; }
        public string MediaURL { get; set; }
        public DateTime DatePosted { get; set; }
        public long Likes { get; set; }
        public long Comments { get; set; }
    }
}