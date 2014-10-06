namespace Infrastructure
{
    using System;

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
                if (value == null)
                {
                    throw new ArgumentNullException("Current");
                }

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

        public static string ToString(DateTime dt)
        {
            return Current.GetToString(dt);
        }

        protected abstract string GetToString(DateTime dt);

        protected abstract DateTime GetNow();

        protected abstract DateTime GetUtcNow();

        public class DefaultTimeProvider : TimeProvider
        {
            protected override string GetToString(DateTime dt)
            {
                return dt.ToString("o");
            }

            protected override DateTime GetNow()
            {
                return DateTime.Now;
            }

            protected override DateTime GetUtcNow()
            {
                return DateTime.UtcNow;
            }
        }

        public class FrozenTimeProvider : TimeProvider
        {
            private readonly DateTime now;
            private readonly DateTime utcNow;

            public FrozenTimeProvider(DateTime now)
                : this(now, now)
            {
            }

            public FrozenTimeProvider(DateTime now, DateTime utcNow)
            {
                this.now = now;
                this.utcNow = utcNow;
            }

            protected override DateTime GetNow()
            {
                return this.now;
            }

            protected override DateTime GetUtcNow()
            {
                return this.utcNow;
            }

            protected override string GetToString(DateTime dt)
            {
                return dt.ToString("o");
            }
        }
    }
}
