namespace TecX.Agile.DesignerSupport
{
    using TecX.Event;

    internal class NullEventAggregator : IEventAggregator
    {
        public void Subscribe(object subscriber)
        {
        }

        public void Unsubscribe(object subscriber)
        {
        }

        public TMessage Publish<TMessage>(TMessage message) where TMessage : class
        {
            return message;
        }
    }
}