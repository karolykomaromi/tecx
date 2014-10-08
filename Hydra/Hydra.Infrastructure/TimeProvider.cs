namespace Hydra.Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class TimeProvider
    {
        private static TimeProvider current = new DefaultTimeProvider();

        public static TimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                Contract.Requires(value != null);

                current = value;
            }
        }

        public static DateTime Now
        {
            get { return Current.GetNow(); }
        }

        public static DateTime UtcNow
        {
            get { return Current.GetUtcNow(); }
        }

        public static DateTime Today
        {
            get { return Current.GetNow().Date; }
        }

        protected abstract DateTime GetNow();

        protected abstract DateTime GetUtcNow();
    }
}
