using System;
using System.Diagnostics;

using Caliburn.Micro;

using TecX.Common;

namespace TecX.Agile
{
    public class DebugLogger : ILog
    {
        private readonly Type _type;

        public DebugLogger(Type type)
        {
            Guard.AssertNotNull(type, "type");

            _type = type;
        }

        private string CreateLogMessage(string format, params object[] args)
        {
            string message = "";
            if (args.Length == 1)
            {
                message = string.Format("[{0} - {1}]",
                    string.Format(format, args[0]),
                    DateTime.Now.ToString("T"));
            }
            else if (args.Length == 2)
            {
                message = string.Format("[{0} - {1}] {2}",
                    args[1],
                    DateTime.Now.ToString("T"),
                    string.Format(format, ((object[])args[0])));
            }

            return message;
        }

        public void Info(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args, "INFO"));
        }

        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args, "WARN"));
        }

        public void Error(Exception exception)
        {
            Debug.WriteLine(CreateLogMessage(exception.ToString(), "ERROR"));
        }
    }
}