using System;

namespace TecX.Common
{   
    public class Latch
    {
        private int _count = 0;

        public bool IsLatched
        {
            get { return _count > 0; }
        }

        private void Increment()
        {
            _count++;
        }

        private void Decrement()
        {
            _count--;
        }

        public void RunOpThatMightRaiseRunawayEvents(Action action)
        {
            Guard.AssertNotNull(action, "action");

            Increment();

            try
            {
                action();
            }
            finally
            {
                Decrement();
            }
        }

        public void RunOpProtectedByLatch(Action action)
        {
            Guard.AssertNotNull(action, "action");

            if (IsLatched)
            {
                return;
            }

            RunOpThatMightRaiseRunawayEvents(action);
        }
    }
}