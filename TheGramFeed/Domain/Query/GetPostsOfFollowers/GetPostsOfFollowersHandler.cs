using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using TheGramFeed.Domain.Command.CreateNewFeedUserProfile;
using TheGramFeed.Domain.DTO.Request;
using TheGramFeed.Domain.DTO.Response;
using TheGramFeed.EventBus;
using TheGramFeed.EventBus.Channels;
using TheGramFeed.Properties;
using TheGramFeed.Repository;

namespace TheGramFeed.Domain.Query.GetPostsOfFollowers
{
    public class GetPostsOfFollowersHandler : IRequestHandler<GetPostsOfFollowersQuery, List<FeedPostsResponse>>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly FeedContext _feedContext;
        private readonly RabbitMQRemoteProcedureCall<List<FeedPostsResponse>> _rabbit;
        private readonly IMediator _mediator;

        public GetPostsOfFollowersHandler(FeedContext feedContext, IMediator mediator)
        {
            _feedContext = feedContext;
            _mediator = mediator;
            _rabbit = new RabbitMQRemoteProcedureCall<List<FeedPostsResponse>>(RabbitMqChannels.GetPostsOfFollowers);
        }

        public async Task<List<FeedPostsResponse>> Handle(GetPostsOfFollowersQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _feedContext.Users.Where(u => u.UserId.Equals(request.UserId)).Include(u => u.Following)
                           .FirstOrDefaultAsync(cancellationToken) ??
                       await _mediator.Send(new CreateNewFeedUserProfileCommand(request.UserId), cancellationToken);

            var followersAsString = user.Following.Select(u => u.UserId).ToList();
            var serializedRequest =
                JsonConvert.SerializeObject(new PaginatedFeedPostsRequest(request.Page, followersAsString));
            
            var response = _rabbit.MakeRemoteProcedureCall<List<FeedPostsResponse>>(serializedRequest,cancellationToken);
            _rabbit.Dispose();
            return response;
        }
    }
}