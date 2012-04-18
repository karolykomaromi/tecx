namespace TecX.Common.Time
{
    using System;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// If you ever need to &quot;change time&quot; be it for tests or some kind of simulation you will have a 
    /// hard time doing so if you use <see cref="DateTime.Now"/> all over your code. This class allows you to 
    /// change how time behaves at your will
    /// </summary>
    public abstract class TimeProvider
    {
        private static readonly object SyncRoot = new object();

        private static TimeProvider current;

        /// <summary>
        /// Initializes static members of the <see cref="TimeProvider"/> class.
        /// </summary>
        static TimeProvider()
        {
            current = new DefaultTimeProvider();
        }

        /// <summary>
        /// Gets or sets the current <see cref="TimeProvider"/>
        /// </summary>
        /// <value>The current provider.</value>
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

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time 
        /// on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public abstract DateTime GetUtcNow();

        /// <summary>
        /// Gets a <see cref="DateTime"/>  object that is set to the current date and time on this computer, 
        /// expressed as the local time.
        /// </summary>
        public abstract DateTime GetNow();
    }
}