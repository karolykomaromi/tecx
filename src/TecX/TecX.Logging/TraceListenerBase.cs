namespace TecX.Logging
{
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    public abstract class TraceListenerBase : CustomTraceListener
    {
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            LogEntry entry = data as LogEntry;

            if (entry != null && 
                this.Formatter != null)
            {
                this.WriteLine(Formatter.Format(entry));
            } 
            else
            {
                this.WriteLine(data.ToString());
            }
        }
    }
}