using System.Collections.Generic;

namespace TheGramFeed.Domain.Models
{
    public class User
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public List<Follower> Following { get; set; } = new List<Follower>();

        public void UpdateUsersFollowingList(string userId)
        {
            var index = Following.FindIndex(f => f.UserId.Equals(userId));
            if (index == -1)
            {
                Following.Add(new Follower {UserId = userId});
            }
            else
            {
                Following.RemoveAt(index);
            }
        }
    }

    public class Follower
    {
        public long Id { get; set; }
        public string UserId { get; set; }
    }
}