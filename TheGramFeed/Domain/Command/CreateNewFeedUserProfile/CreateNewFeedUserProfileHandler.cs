using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramFeed.Domain.Models;
using TheGramFeed.Repository;

namespace TheGramFeed.Domain.Command.CreateNewFeedUserProfile
{
    public class CreateNewFeedUserProfileHandler : IRequestHandler<CreateNewFeedUserProfileCommand, User>
    {
        private readonly FeedContext _feedContext;

        public CreateNewFeedUserProfileHandler(FeedContext feedContext)
        {
            _feedContext = feedContext;
        }
        public async Task<User> Handle(CreateNewFeedUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _feedContext.Users.Where(u => u.UserId.Equals(request.UserId)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user != null)
            {
                throw new Exception($"User with id {request.UserId} already exists");
            }
            user = new User {UserId = request.UserId};
            // Add self to list of users the user follows for easier fetching
            user.UpdateUsersFollowingList(user.UserId);
            await _feedContext.Users.AddAsync(user, cancellationToken);
            await _feedContext.SaveChangesAsync();
            return user;
        }
    }
}