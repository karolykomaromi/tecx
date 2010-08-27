namespace TecX.Common.Event
{
    public interface IEventAggregator
    {
        void Subscribe(object subscriber);

        void Unsubscribe(object subscriber);

        void Publish<TMessage>(TMessage message);

        ICancellationToken PublishWithCancelOption<TMessage>(TMessage message)
            where TMessage : ICancellationToken;
    }
}