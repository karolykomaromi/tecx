namespace TecX.Agile.Messaging
{
    using System;

    using TecX.Common;

    public class MessagePublisher
    {
        private readonly IMessageChannel channel;

        public MessagePublisher(IMessageChannel channel)
        {
            Guard.AssertNotNull(channel, "channel");

            this.channel = channel;
        }

        public void Publish(object message)
        {
            Guard.AssertNotNull(message, "message");

            Type messageType = message.GetType();

            var method = typeof(IMessageChannel).GetMethod("Send");

            var publish = method.MakeGenericMethod(messageType);

            publish.Invoke(this.channel, new[] { message });
        }
    }
}
