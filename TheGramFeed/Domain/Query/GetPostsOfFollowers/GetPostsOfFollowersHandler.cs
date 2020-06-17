using System;
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
using TheGramFeed.Properties;
using TheGramFeed.Repository;

namespace TheGramFeed.Domain.Query.GetPostsOfFollowers
{
    public class GetPostsOfFollowersHandler : IRequestHandler<GetPostsOfFollowersQuery, List<FeedPostsResponse>>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly FeedContext _feedContext;
        private readonly RabbitRPC<List<FeedPostsResponse>> _rabbit;
        private readonly IMediator _mediator;

        public GetPostsOfFollowersHandler(FeedContext feedContext, IMediator mediator)
        {
            _feedContext = feedContext;
            _mediator = mediator;
            _rabbit = new RabbitRPC<List<FeedPostsResponse>>(RabbitMqChannels.GetPostsOfFollowers);
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
            Console.WriteLine(serializedRequest);
            try
            {
                var task = Task.Run(() => _rabbit.Request<List<FeedPostsResponse>>(serializedRequest),
                    cancellationToken);
                return task.Wait(TimeSpan.FromSeconds(2)) ? task.Result : new List<FeedPostsResponse>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}