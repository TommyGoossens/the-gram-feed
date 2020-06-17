using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheGramFeed.Domain.DTO.Response;
using TheGramFeed.Domain.Query.GetPostsOfFollowers;
using TheGramFeed.Helpers;

namespace TheGramFeed.Controllers
{
    [Route("api/feed")]
    [Authorize]
    public class FeedController : ControllerBase
    {
        private readonly IUserContextHelper _userContextHelper;
        private readonly IMediator _mediator;

        public FeedController(IUserContextHelper userContextHelper, IMediator mediator)
        {
            _userContextHelper = userContextHelper;
            _mediator = mediator;
        }

        [HttpGet("{page}")]
        public async Task<List<FeedPostsResponse>> GetAllPostsOfFollowedUsers(int page)
        {
            var userId = _userContextHelper.GetUserId();
            return await _mediator.Send(new GetPostsOfFollowersQuery(userId,page));
        }
    }
}