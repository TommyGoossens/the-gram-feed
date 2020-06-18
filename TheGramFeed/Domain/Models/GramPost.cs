using System;

namespace TheGramFeed.Domain.Models
{
    public class GramPost
    {
        public User User { get; set; }
        public string MediaURL { get; set; }
        public DateTime DatePosted { get; set; }
        public long Likes { get; set; }
        public long Comments { get; set; }
    }
}