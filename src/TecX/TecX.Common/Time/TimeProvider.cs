namespace TecX.Common.Time
{
    using System;

    /// <summary>
    /// If you ever need to &quot;change time&quot; be it for tests or some kind of simulation you will have a 
    /// hard time doing so if you use <see cref="DateTime.Now"/> all over your code. This class allows you to 
    /// change how time behaves at your will
    /// </summary>
    public abstract class TimeProvider
    {
        private static readonly object SyncRoot = new object();

        private static TimeProvider current;

        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        public static TimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                Guard.AssertNotNull(value, "Current");

                lock (SyncRoot)
                {
                    current = value;
                }
            }
        }

        public static DateTime Now
        {
            get
            {
                return Current.GetNow();
            }
        }

        public static DateTime UtcNow
        {
            get
            {
                return Current.GetUtcNow();
            }
        }

        public static string ToString(DateTime dt)
        {
            return Current.GetString(dt);
        }

        protected abstract string GetString(DateTime dt);

        protected abstract DateTime GetUtcNow();

        protected abstract DateTime GetNow();
    }
}