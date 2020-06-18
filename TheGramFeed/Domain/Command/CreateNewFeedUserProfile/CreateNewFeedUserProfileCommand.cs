using MediatR;
using TheGramFeed.Domain.Models;

namespace TheGramFeed.Domain.Command.CreateNewFeedUserProfile
{
    public class CreateNewFeedUserProfileCommand : IRequest<User>
    {
        public string UserId { get; }

        public CreateNewFeedUserProfileCommand(string userId)
        {
            UserId = userId;
        }
    }
}