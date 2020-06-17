using System.Collections.Generic;
using MediatR;
using TheGramFeed.Domain.DTO.Response;

namespace TheGramFeed.Domain.Query.GetPostsOfFollowers
{
    public class GetPostsOfFollowersQuery : IRequest<List<FeedPostsResponse>>
    {
        public string UserId { get; }
        public int Page { get; }

        public GetPostsOfFollowersQuery(string userId, int page)
        {
            UserId = userId;
            Page = page;
        }
    }
}