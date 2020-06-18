using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheGramFeed.Domain.Models;

namespace TheGramFeed.Repository
{
    public class FeedContext : DbContext, IFeedContext
    {
        public DbSet<User> Users { get; set; }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        public FeedContext(DbContextOptions<FeedContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}