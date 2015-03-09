namespace Hydra.Infrastructure.Logging
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;

    public static class DebugLog
    {
        public static SinkSubscription<DebugSink> LogToDebug(this IObservable<EventEntry> eventStream, IEventTextFormatter formatter = null)
        {
            Contract.Requires(eventStream != null);
            Contract.Ensures(Contract.Result<SinkSubscription<DebugSink>>() != null);

            formatter = formatter ?? new EventTextFormatter();

            var sink = new DebugSink(formatter);

            IDisposable subscription = eventStream.Subscribe(sink);

            return new SinkSubscription<DebugSink>(subscription, sink);
        }
    }
}