﻿namespace TecX.Logging
{
    using System.Diagnostics;

    public class DebugTraceListener : TraceListenerBase
    {
        #region Overrides of TraceListener

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. </param><filterpriority>2</filterpriority>
        public override void Write(string message)
        {
            Debug.Write(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. </param><filterpriority>2</filterpriority>
        public override void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        #endregion
    }
}