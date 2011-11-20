namespace TecX.Event
{
    public interface IEventAggregator
    {
        void Subscribe(object subscriber);

        void Unsubscribe(object subscriber);

        TMessage Publish<TMessage>(TMessage message) where TMessage : class;
    }
}