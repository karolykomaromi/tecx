namespace TecX.Common
{
    using System;

    public class Latch
    {
        private int count = 0;

        public bool IsLatched
        {
            get { return this.count > 0; }
        }

        private void Increment()
        {
            this.count++;
        }

        private void Decrement()
        {
            this.count--;
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