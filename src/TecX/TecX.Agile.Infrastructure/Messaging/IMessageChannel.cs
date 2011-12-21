namespace TecX.Agile.Infrastructure.Messaging
{
    public interface IMessageChannel
    {
        void Subscribe(object subscriber);

        void Unsubscribe(object subscriber);

        void Publish<TMessage>(TMessage message) where TMessage : class;
    }
}