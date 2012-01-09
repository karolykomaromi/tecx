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

        public void RunOpThatMightRaiseRunawayEvents(Action action)
        {
            Guard.AssertNotNull(action, "action");

            this.Increment();

            try
            {
                action();
            }
            finally
            {
                this.Decrement();
            }
        }

        public void RunOpProtectedByLatch(Action action)
        {
            Guard.AssertNotNull(action, "action");

            if (this.IsLatched)
            {
                return;
            }

            this.RunOpThatMightRaiseRunawayEvents(action);
        }

        private void Increment()
        {
            this.count++;
        }

        private void Decrement()
        {
            this.count--;
        }
    }
}