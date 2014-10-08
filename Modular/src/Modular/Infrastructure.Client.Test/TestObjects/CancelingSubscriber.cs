namespace Infrastructure.Client.Test.TestObjects
{
    using System;

    using Infrastructure.Events;

    public class CancelingSubscriber : ISubscribeTo<CancelMessage>
    {
        private readonly Action action;

        public CancelingSubscriber(Action action)
        {
            this.action = action;
        }

        public void Handle(CancelMessage message)
        {
            message.Cancel = true;
            this.action();
        }
    }
}