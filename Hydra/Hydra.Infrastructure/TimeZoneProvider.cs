namespace Hydra.Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class TimeZoneProvider
    {
        private static TimeZoneProvider current = new DefaultTimeZoneProvider();

        public static TimeZoneProvider Current
        {
            get
            {
                Contract.Ensures(Contract.Result<TimeZoneProvider>() != null);

                return current;
            }

            set
            {
                Contract.Requires(value != null);

                current = value;
            }
        }

        public static TimeZoneInfo Local
        {
            get { return TimeZoneProvider.Current.GetLocal(); }
        }

        public static TimeZoneInfo Utc
        {
            get { return TimeZoneProvider.Current.GetUtc(); }
        }

        protected abstract TimeZoneInfo GetLocal();

        protected abstract TimeZoneInfo GetUtc();
    }
}