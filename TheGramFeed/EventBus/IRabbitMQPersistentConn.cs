using RabbitMQ.Client;

namespace TheGramFeed.EventBus
{
    public interface IRabbitMQPersistentConn
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void CreateConsumerChannel();

        void Disconnect();
    }
}