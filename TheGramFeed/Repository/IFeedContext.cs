using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheGramFeed.Domain.Models;

namespace TheGramFeed.Repository
{
    public interface IFeedContext
    {
        public DbSet<User> Users { get; set; }
        Task SaveChangesAsync();
    }
}