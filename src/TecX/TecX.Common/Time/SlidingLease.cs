namespace TecX.Common.Time
{
    using System;

    public class SlidingLease : ILease
    {
        private readonly TimeSpan timeout;
        private DateTime renewed;

        public SlidingLease(TimeSpan timeout)
        {
            this.timeout = timeout;
            this.renewed = TimeProvider.Current.Now;
        }

        public TimeSpan Timeout
        {
            get { return this.timeout; }
        }

        public bool IsExpired
        {
            get
            {
                return TimeProvider.Current.Now > this.renewed + this.timeout;
            }
        }

        public void Renew()
        {
            this.renewed = TimeProvider.Current.Now;
        }
    }
}