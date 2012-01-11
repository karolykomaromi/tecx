namespace TecX.Agile.Messaging.Test.TestObjects
{
    using System;
    using System.Collections.Generic;

    public class TestMessageChannel : IMessageChannel
    {
        private readonly List<MessageHub> hubs;

        public TestMessageChannel()
        {
            this.hubs = new List<MessageHub>();
        }

        public void Subscribe(object subscriber)
        {
            this.hubs.Add((MessageHub)subscriber);
        }

        public void Unsubscribe(object subscriber)
        {
            throw new NotImplementedException();
        }

        public void Send<TMessage>(TMessage message) where TMessage : class
        {
            Type messageType = message.GetType();

            var method = typeof(MessageHub).GetMethod("Handle", new[] { messageType });

            method.Invoke(this.hubs[1], new object[] { message });
        }
    }
}