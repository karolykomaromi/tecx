using TecX.Common.Event;

namespace TecX.Agile.Infrastructure
{
    public class NullEventAggregator : IEventAggregator
    {
        #region Implementation of IEventAggregator

        public void Subscribe(object subscriber)
        {
        }

        public void Unsubscribe(object subscriber)
        {
        }

        public TMessage Publish<TMessage>(TMessage message)
            where TMessage : class
        {
            return null;
        }

        #endregion
    }
}