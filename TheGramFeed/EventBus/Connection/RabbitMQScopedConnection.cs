using System;
using RabbitMQ.Client;

namespace TheGramFeed.EventBus.Connection
{
    public abstract class RabbitMQScopedConnection : RabbitMQBaseConnection, IDisposable
    {
        protected readonly IModel Channel;

        protected RabbitMQScopedConnection()
        {
            Channel = Connection.CreateModel();
        }

        public abstract void Dispose();
    }
}