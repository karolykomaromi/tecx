namespace TecX.Common
{
    public delegate void VoidHandler();

    public class Latch
    {
        #region Fields

        private int _count = 0;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        public bool IsLatched
        {
            get { return _count > 0; }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region Methods

        public void Increment()
        {
            _count++;
        }

        public void Decrement()
        {
            _count--;
        }

        public void RunOpThatMightRaiseRunawayEvents(VoidHandler handler)
        {
            Increment();

            try
            {
                handler();
            }
            finally
            {
                Decrement();
            }
        }

        public void RunOpProtectedByLatch(VoidHandler handler)
        {
            if (IsLatched)
            {
                return;
            }

            handler();
        }

        #endregion Methods
    }
}