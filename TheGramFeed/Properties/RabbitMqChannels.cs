namespace TheGramFeed.Properties
{
    public static class RabbitMqChannels
    {
        public const string GetPostsOfFollowers = "get_posts_of_followers_q";
        public const string UpdateFollowerForUser = "update_follower_for_user_q";
        public const string TopicFollowerUpdate = "user.follower.update";
    }
}