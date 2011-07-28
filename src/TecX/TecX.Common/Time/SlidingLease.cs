using System;

namespace TecX.Common.Time
{
    public class SlidingLease : ILease
    {
        private readonly TimeSpan _timeout;
        private DateTime _renewed;

        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public bool IsExpired
        {
            get
            {
                return TimeProvider.Current.Now > _renewed + _timeout;
            }
        }

        public SlidingLease(TimeSpan timeout)
        {
            _timeout = timeout;
            _renewed = TimeProvider.Current.Now;
        }

        public void Renew()
        {
            _renewed = TimeProvider.Current.Now;
        }
    }
}