using RabbitMQ.Client;

namespace TheGramFeed.EventBus
{
    public interface IEventBusService
    {
        public IModel CreateConsumerChannel();
    }
}