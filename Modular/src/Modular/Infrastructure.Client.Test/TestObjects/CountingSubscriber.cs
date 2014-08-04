namespace Infrastructure.Client.Test.TestObjects
{
    using System;
    using System.Threading;

    using Infrastructure.Events;

    public class CountingSubscriber : ISubscribeTo<Message>
    {
        private readonly Action action;

        private int messageCounter = 0;

        public CountingSubscriber(Action action)
        {
            this.action = action;
        }

        public int MessageCounter
        {
            get
            {
                return this.messageCounter;
            }
        }

        public void Handle(Message message)
        {
            Interlocked.Increment(ref this.messageCounter);

            this.action();
        }
    }
}