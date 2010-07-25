using System;

namespace TecX.Common
{
    /// <summary>
    /// If you ever need to &quot;change time&quot; be it for tests or some kind of simulation you will have a 
    /// hard time doing so if you use <see cref="DateTime.Now"/> all over your code. This class allows you to 
    /// change how time behaves at your will
    /// </summary>
    public abstract class TimeProvider
    {
        #region Fields

        private static readonly object SyncRoot = new object();

        private static TimeProvider _current;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        static TimeProvider()
        {
            _current = new DefaultTimeProvider();
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the current <see cref="TimeProvider"/>
        /// </summary>
        /// <value>The current provider.</value>
        public static TimeProvider Current
        {
            get
            {
                return _current;
            }
            set
            {
                Guard.AssertNotNull(value, "value", "An Ambient Context must never be null");

                lock (SyncRoot)
                {
                    _current = value;
                }
            }
        }

        /// <summary>
        /// Resets the current provider to a &quot;good local default&quot;.
        /// </summary>
        public static void ResetToDefault()
        {
            Current = new DefaultTimeProvider();
        }

        /// <summary>
        ///Gets a <see cref="DateTime"/> object that is set to the current date and time 
        /// on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public abstract DateTime UtcNow { get; }

        /// <summary>
        /// Gets a <see cref="DateTime"/>  object that is set to the current date and time on this computer, 
        /// expressed as the local time.
        /// </summary>
        public abstract DateTime Now { get; }

    }

    /// <summary>
    /// Default implementation of a <see cref="TimeProvider"/> using <see cref="DateTime"/> internally
    /// </summary>
    public class DefaultTimeProvider : TimeProvider
    {
        #region Overrides of TimeProvider

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time
        /// on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value></value>
        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/>  object that is set to the current date and time on this computer,
        /// expressed as the local time.
        /// </summary>
        /// <value></value>
        public override DateTime Now
        {
            get { return DateTime.Now; }
        }

        #endregion Overrides of TimeProvider
    }
}