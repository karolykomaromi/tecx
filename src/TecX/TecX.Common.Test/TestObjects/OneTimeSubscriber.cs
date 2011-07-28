using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Event;

namespace TecX.Common.Test.TestObjects
{
    internal class OneTimeSubscriber : ISubscribeTo<SimpleMessage>
    {
        public int Counter = 0;

        public void Handle(SimpleMessage message)
        {
            Interlocked.Increment(ref Counter);

            if (Counter == 2)
                Assert.Fail("must not be called twice");
        }
    }
}