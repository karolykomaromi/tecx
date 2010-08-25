namespace TecX.Common.Event
{
    public interface IEventAggregator
    {
        void Subscribe(object subscriber);

        void Unsubscribe(object subscriber);

        void Publish<TMessage>(TMessage message);
    }
}