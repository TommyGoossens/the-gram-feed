using System;
using NLog;
using TheGramFeed.EventBus.Channels;
using TheGramFeed.Properties;

namespace TheGramFeed.EventBus.Connection
{
    public class RabbitMqPersistentConn : RabbitMQBaseConnection
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private RabbitMQAbstractChannel _topicUserFollowerUpdateChannel;

        //Used to create a Mediator service in a singleton
        protected readonly IServiceProvider ServiceProvider;

        public RabbitMqPersistentConn(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void CreatePersistentChannels()
        {
            if (!IsConnected)
            {
                Logger.Error("No connection while creating consumer channels, retrying.");
                TryConnect();
            }

            _topicUserFollowerUpdateChannel = new RabbitMQExchangeChannel(this, "topic_user",
                new[] {RabbitMqChannels.TopicFollowerUpdate});
            _topicUserFollowerUpdateChannel.DeclareChannel();
        }
    }
}