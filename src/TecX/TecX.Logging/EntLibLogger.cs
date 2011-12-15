namespace TecX.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using Microsoft.Practices.EnterpriseLibrary.Logging;

    using TecX.Common;

    public class EntLibLogger : ILogger
    {
        private const int DefaultPriority = -1;
        private const int DefaultEventId = 1;
        private static readonly ICollection<string> emptyCategoriesList = new string[0];

        private readonly LogWriter logWriter;

        public EntLibLogger(LogWriter logWriter)
        {
            Guard.AssertNotNull(logWriter, "logWriter");

            this.logWriter = logWriter;
        }

        public void Verbose(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            if (this.logWriter.IsLoggingEnabled())
            {
                this.logWriter.Write(message, emptyCategoriesList, DefaultPriority, DefaultEventId, TraceEventType.Verbose);
            }
        }

        public void Verbose(string format, params object[] args)
        {
            Guard.AssertNotEmpty(format, "format");

            if (this.logWriter.IsLoggingEnabled())
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);

                this.Verbose(message);
            }
        }

        public void Info(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            if (this.logWriter.IsLoggingEnabled())
            {
                this.logWriter.Write(message, emptyCategoriesList, DefaultPriority, DefaultEventId, TraceEventType.Information);
            }
        }

        public void Info(string format, params object[] args)
        {
            Guard.AssertNotEmpty(format, "format");

            if (this.logWriter.IsLoggingEnabled())
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);

                this.Info(message);
            }
        }

        public void Warning(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            if (this.logWriter.IsLoggingEnabled())
            {
                this.logWriter.Write(message, emptyCategoriesList, DefaultPriority, DefaultEventId, TraceEventType.Warning);
            }
        }

        public void Warning(string format, params object[] args)
        {
            Guard.AssertNotEmpty(format, "format");

            if (this.logWriter.IsLoggingEnabled())
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);

                this.Warning(message);
            }
        }

        public void Error(Exception exception)
        {
            Guard.AssertNotNull(exception, "exception");

            this.Error(exception, "An error occurred.");
        }

        public void Error(Exception exception, string message)
        {
            Guard.AssertNotEmpty(message, "message");

            if (this.logWriter.IsLoggingEnabled())
            {
                string ex = ExceptionCrawler.ExtractErrorMessages(exception);

                this.logWriter.Write(message + Environment.NewLine + ex, emptyCategoriesList, DefaultPriority, DefaultEventId, TraceEventType.Error);
            }
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Guard.AssertNotEmpty(format, "format");

            if (this.logWriter.IsLoggingEnabled())
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);

                this.Error(exception, message);
            }
        }

        public void Error(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            if (this.logWriter.IsLoggingEnabled())
            {
                this.logWriter.Write(message, emptyCategoriesList, DefaultPriority, DefaultEventId, TraceEventType.Error);
            }
        }

        public void Error(string format, params object[] args)
        {
            Guard.AssertNotEmpty(format, "format");

            if (this.logWriter.IsLoggingEnabled())
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);

                this.Error(message);
            }
        }
    }
}
