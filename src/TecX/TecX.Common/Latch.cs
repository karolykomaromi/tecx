using System;

namespace TecX.Common
{   
    public class Latch
    {
        #region Fields

        private int _count = 0;

        #endregion Fields

        #region Properties

        public bool IsLatched
        {
            get { return _count > 0; }
        }

        #endregion Properties

        #region Methods

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

            action();
        }

        #endregion Methods
    }
}