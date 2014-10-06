namespace TecX.Agile.Messaging
{
    public interface IMessageChannel
    {
        void Subscribe(object subscriber);

        void Unsubscribe(object subscriber);

        void Send<TMessage>(TMessage message) where TMessage : class;
    }
}