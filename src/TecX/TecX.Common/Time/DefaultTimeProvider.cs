namespace TecX.Common.Time
{
    using System;

    /// <summary>
    /// Default implementation of a <see cref="TimeProvider"/> using <see cref="DateTime"/> internally
    /// </summary>
    public class DefaultTimeProvider : TimeProvider
    {
        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time
        /// on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns> </returns>
        protected override DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/>  object that is set to the current date and time on this computer,
        /// expressed as the local time.
        /// </summary>
        /// <returns> </returns>
        protected override DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}