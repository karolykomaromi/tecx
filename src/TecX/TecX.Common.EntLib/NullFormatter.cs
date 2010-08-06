using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace TecX.Common.EntLib
{
    /// <summary>
    /// Always returns an empty string. Should only be used in combination with the <see cref="NullTraceListener"/>
    /// </summary>
    public class NullFormatter : LogFormatter
    {
        /// <summary>
        /// Formats a log entry and return a string to be outputted.
        /// </summary>
        /// <param name="log">Log entry to format.</param>
        /// <returns>A string representing the log entry.</returns>
        public override string Format(LogEntry log)
        {
            return string.Empty;
        }
    }
}