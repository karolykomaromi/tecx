namespace Cars
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

        public static DateTimeOffset Now
        {
            get { return Current.GetNow(); }
        }

        public static DateTimeOffset UtcNow
        {
            get { return Current.GetUtcNow(); }
        }

        public static DateTimeOffset Today
        {
            get { return Current.GetNow().Date; }
        }

        protected abstract DateTimeOffset GetNow();

        protected abstract DateTimeOffset GetUtcNow();
    }
}
