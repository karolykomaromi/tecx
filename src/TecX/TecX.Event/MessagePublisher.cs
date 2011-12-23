namespace TecX.Event
{
    using System;

    using TecX.Common;

    public class MessagePublisher
    {
        private readonly IEventAggregator eventAggregator;

        public MessagePublisher(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;
        }

        public void Publish(object message)
        {
            Guard.AssertNotNull(message, "message");

            Type messageType = message.GetType();

            var method = typeof(IEventAggregator).GetMethod("Publish");

            var publish = method.MakeGenericMethod(messageType);

            publish.Invoke(this.eventAggregator, new[] { message });
        }
    }
}