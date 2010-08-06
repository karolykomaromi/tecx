using System.Text;

namespace TecX.Common.EntLib
{
    public class InMemoryTraceListener : TraceListenerBase
    {
        private static readonly StringBuilder Messages = new StringBuilder(8192);
        private static readonly object SyncRoot = new object();

        public override void Write(string message)
        {
            Guard.AssertNotNull(message, "message");

            lock (SyncRoot)
            {
                Messages.Append(message);
            }
        }

        public override void WriteLine(string message)
        {
            Guard.AssertNotNull(message, "message");

            lock (SyncRoot)
            {
                Messages.AppendLine(message);
            }
        }
    }
}