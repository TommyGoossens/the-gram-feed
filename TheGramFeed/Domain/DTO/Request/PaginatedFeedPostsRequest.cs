using System.Collections.Generic;

namespace TheGramFeed.Domain.DTO.Request
{
    public class PaginatedFeedPostsRequest
    {
        public int Page { get; }
        public List<string> FollowedUsers { get; }

        public PaginatedFeedPostsRequest(int page, List<string> followedUsers)
        {
            Page = page;
            FollowedUsers = followedUsers;
        }
    }
}