using System.Threading.Tasks;

namespace TheGramFeed.Helpers
{
    public interface IUserContextHelper
    {
        public Task<string> GetAuthToken();
        public string GetUserId();
    }
}