using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace TecX.Common.EntLib
{
    public abstract class TecXTraceListenerBase : CustomTraceListener
    {
        public override void TraceData(TraceEventCache eventCache, 
            string source, 
            TraceEventType eventType, 
            int id,
            object data)
        {
            LogEntry entry = data as LogEntry;

            if (entry != null && Formatter != null)
            {
                WriteLine(Formatter.Format(entry));
            } 
            else
            {
                WriteLine(data.ToString());
            }
        }
    }
}