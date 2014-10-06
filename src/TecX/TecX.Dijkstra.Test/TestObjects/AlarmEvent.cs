using System.Diagnostics;

namespace TecX.Dijkstra.Test.TestObjects
{
    [DebuggerDisplay("{Message}")]
    public class AlarmEvent
    {
        private readonly AlarmEventType eventType;
        private readonly string message;

        public AlarmEvent(AlarmEventType type, string message)
        {
            eventType = type;
            this.message = message;
        }

        public AlarmEventType EventType
        {
            get { return eventType; }
        }

        public string Message
        {
            get { return message; }
        }
    }
}