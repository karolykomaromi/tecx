using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace TecX.Common.EntLib
{
    /// <summary>
    /// Implements a simple <see cref="CustomTraceListener"/> that writes to <see cref="Debug"/>
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class DebugTraceListener : TraceListenerBase
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugTraceListener"/> class.
        /// </summary>
        public DebugTraceListener()
        {

        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Overrides of CustomTraceListener

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. 
        /// </param><filterpriority>2</filterpriority>
        public override void Write(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Debug.Write(message);
            }
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void WriteLine(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Debug.WriteLine(message);
            }
        }

        #endregion Overrides of CustomTraceListener
    }
}