namespace Hydra.Infrastructure.Logging
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;

    public class DebugSink : IObserver<EventEntry>
    {
        private readonly IEventTextFormatter formatter;

        public DebugSink(IEventTextFormatter formatter)
        {
            Contract.Requires(formatter != null);

            this.formatter = formatter;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// Provides the sink with new data to write.
        /// </summary>
        /// <param name="value">The current entry and its color to write to the console.</param>
        public void OnNext(EventEntry value)
        {
            string convertedValue = value.TryFormatAsString(this.formatter);
            if (convertedValue != null)
            {
                OnNext(convertedValue);
            }
        }

        private static void OnNext(string entry)
        {
            Debug.Write(entry);
            Debug.Flush();
        }
    }
}