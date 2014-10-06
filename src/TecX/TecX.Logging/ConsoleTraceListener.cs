namespace TecX.Logging
{
    using System;
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    using TecX.Common;

    /// <summary>
    /// Writes logging information to the console
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class ConsoleTraceListener : CustomTraceListener
    {
        /// <summary>
        /// Creates a new <see cref="ConsoleTraceListener"/> instance
        /// </summary>
        public ConsoleTraceListener()
        {
            // makes sure that a console is allocated for the current thread, even if it is a
            // WinForms or WPF application
            Win32.AllocConsole();
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            Console.Write(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///     <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
        /// </PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            LogEntry entry = data as LogEntry;

            if (entry != null &&
                this.Formatter != null)
            {
                this.TraceData(eventCache, source, eventType, id, this.Formatter.Format(entry));
            }
            else
            {
                base.TraceData(eventCache, source, eventType, id, data);
            }
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///     <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
        /// </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            SwitchForegroundColor(eventType);

            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <summary>
        /// Overrides <see cref="TraceListener.Dispose(bool)"/>
        /// Assures that the console buffer is flushed when the object is disposed.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Console.Out.Flush();

            Win32.FreeConsole();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Changes the color of the text on the console depending on the type of event that
        /// is logged.
        /// </summary>
        /// <param name="eventType">The type of event that shall be logged</param>
        private static void SwitchForegroundColor(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case TraceEventType.Error:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case TraceEventType.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case TraceEventType.Resume:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TraceEventType.Start:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TraceEventType.Stop:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TraceEventType.Suspend:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TraceEventType.Transfer:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TraceEventType.Verbose:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TraceEventType.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}