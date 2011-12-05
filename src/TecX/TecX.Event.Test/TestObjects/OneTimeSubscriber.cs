namespace TecX.Event.Test.TestObjects
{
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Event;

    public class OneTimeSubscriber : ISubscribeTo<SimpleMessage>
    {
        public int Counter = 0;

        public void Handle(SimpleMessage message)
        {
            Interlocked.Increment(ref Counter);

            if (this.Counter == 2)
            {
                Assert.Fail("must not be called twice");
            }
        }
    }
}