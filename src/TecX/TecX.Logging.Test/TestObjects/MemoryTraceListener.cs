using System.Text;
using TecX.Common;

namespace TecX.Logging.Test.TestObjects
{
    internal class MemoryTraceListener : TraceListenerBase
    {
        private readonly StringBuilder _messages = new StringBuilder(8192);
        private readonly object _syncRoot = new object();

        public string Messages
        {
            get
            {
                lock (_syncRoot)
                {
                    return _messages.ToString();
                }
            }
        }

        public override void Write(string message)
        {
            Guard.AssertNotNull(message, "message");

            lock (_syncRoot)
            {
                _messages.Append(message);
            }
        }

        public override void WriteLine(string message)
        {
            Guard.AssertNotNull(message, "message");

            lock (_syncRoot)
            {
                _messages.AppendLine(message);
            }
        }
    }
}