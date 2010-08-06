namespace TecX.Common.EntLib
{
    /// <summary>
    /// Doesn't do anything. I know, not intuitive for a logger but sometimes you just dont want logging enabled
    /// </summary>
    public class NullTraceListener : TraceListenerBase
    {
        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            /* don't do anything */
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            /* don't do anything */
        }
    }
}