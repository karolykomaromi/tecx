namespace TecX.Logging
{
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    /// <summary>
    /// Implements the NullObject-pattern and does absolutely nothing
    /// </summary>
    public class NullTraceListener : CustomTraceListener
    {
        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }
    }
}
