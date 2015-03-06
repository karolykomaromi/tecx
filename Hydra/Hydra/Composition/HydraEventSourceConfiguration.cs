namespace Hydra.Composition
{
    using System;
    using System.Diagnostics.Tracing;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
    using Microsoft.Practices.Unity;

    public class HydraEventSourceConfiguration : UnityContainerExtension, IDisposable
    {
        private ObservableEventListener listener;
        private IDisposable subscription;
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected override void Initialize()
        {
            if (this.listener != null)
            {
                this.listener.DisableEvents(HydraEventSource.Log);
                this.listener.Dispose();
            }

            if (this.subscription != null)
            {
                this.subscription.Dispose();
            }

            this.listener = new ObservableEventListener();

            this.listener.EnableEvents(HydraEventSource.Log, EventLevel.Verbose, Keywords.All);

            SinkSubscription<DebugSink> sinkSubscription = this.listener.LogToDebug(new JsonEventTextFormatter(EventTextFormatting.Indented, "o"));

            this.subscription = sinkSubscription.Subscription;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.listener != null)
                {
                    this.listener.DisableEvents(HydraEventSource.Log);
                    this.listener.Dispose();
                    this.listener = null;
                }

                if (this.subscription != null)
                {
                    this.subscription.Dispose();
                    this.subscription = null;
                }
            }
        }
    }
}