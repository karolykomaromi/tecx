namespace TecX.Logging
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text;

    using TecX.Common;
    using TecX.Common.Time;

    [Serializable]
    public sealed class TraceTimer : IDisposable
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            /// <summary>{0} (took {1:F4}s)</summary>
            public const string MessageFormatString = "{0} (took {1:F4}s)";

            /// <summary>Trace</summary>
            public const string TraceTimerCategory = "Trace";
        }

        private readonly DateTime start;

        private readonly string operationName;

        public TraceTimer(string operationName, params object[] args)
        {
            Guard.AssertNotEmpty(operationName, "operationName");

            this.operationName = args == null ? operationName : string.Format(operationName, args);

            this.start = TimeProvider.Now;

            Debug.Indent();
        }

        public TraceTimer()
            : this(GetMethodSignature())
        {
        }

        public void Dispose()
        {
            TimeSpan lifetime = TimeProvider.Now - this.start;

            Debug.WriteLine(
                string.Format(Constants.MessageFormatString, this.operationName, lifetime.TotalSeconds),
                Constants.TraceTimerCategory);

            Debug.Unindent();
        }

        private static string GetMethodSignature()
        {
            StackTrace trace = new StackTrace();

            // index 0 is GetMethodSignature()
            // index 1 is call to constructor of TraceTimer
            // index 2 is method where constructor is called
            MethodBase m = trace.GetFrame(2).GetMethod();

            StringBuilder sb = new StringBuilder(100);

            sb.Append(m.Name);
            sb.Append("(");

            foreach (ParameterInfo parameter in m.GetParameters())
            {
                sb.Append(parameter.ParameterType.Name).Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");

            return sb.ToString();
        }
    }
}