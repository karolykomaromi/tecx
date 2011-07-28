using System;

namespace TecX.Common.Time
{
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

        /// <summary>
        /// Gets the current date.
        /// </summary>
        public override DateTime Today
        {
            get { return DateTime.Today; }
        }

        #endregion Overrides of TimeProvider
    }
}