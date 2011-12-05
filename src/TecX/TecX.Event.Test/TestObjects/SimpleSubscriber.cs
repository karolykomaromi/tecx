namespace TecX.Event.Test.TestObjects
{
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Event;

    public class SimpleSubscriber : ISubscribeTo<SimpleMessage>
    {
        public bool MessageReceived = false;
        public int MessageCounter = 0;

        public void Handle(SimpleMessage message)
        {
            Assert.IsNotNull(message);

            MessageReceived = true;
            Interlocked.Increment(ref MessageCounter);
        }
    }
}