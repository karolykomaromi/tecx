namespace TecX.Logging
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using TecX.Common;
    using TecX.Common.Time;

    /// <summary>
    /// Traces the duration of an operation
    /// </summary>
    /// <example>
    /// <para>Enclose a method call with a using statement and pass the methods name to the constructor
    /// of the <i>TraceTimer</i>. If you use overloaded methods passing the signature of the method
    /// helps recognizing which one was called.
    /// </para>
    /// <c>
    /// using(new TraceTimer("MyNamespace.MyMethod(string,string)")
    /// {
    ///     MyMethod("a","string");
    /// }
    /// </c>
    /// </example>
    [Serializable]
    public sealed class TraceTimer : IDisposable
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            /// <summary>"Trace - {0} (took {1}s)"</summary>
            public const string MessageFormatString = "Trace - {0} (took {1}s)";

            /// <summary>Trace</summary>
            public const string TraceTimerCategory = "Trace";
        }

        private readonly DateTime start;

        private readonly string operationName;

        /// <summary>
        /// Creates a new <see cref="TraceTimer"/> instance
        /// </summary>
        public TraceTimer(string operationName)
        {
            this.start = TimeProvider.Now;
            this.operationName = operationName;

            Debug.Indent();
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~TraceTimer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>          
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Traces the message and the current lifespan of the object
        /// </summary>
        /// <param name="message">The message to trace</param>
        public void Write(string message)
        {
            TimeSpan lifetime = TimeProvider.Now - this.start;

            Debug.WriteLine(
                TypeHelper.SafeFormat(Constants.MessageFormatString, message, lifetime.TotalSeconds),
                Constants.TraceTimerCategory);
        }

        /// <summary>
        /// Disposes the object. On dispose the lifespan of the object is traced.
        /// </summary>
        /// <param name="disposing">If <i>false</i>, cleans up native resources.
        /// If <i>true</i> cleans up both managed and native resources</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Write(this.operationName);

                Debug.Unindent();
            }
        }
    }
}