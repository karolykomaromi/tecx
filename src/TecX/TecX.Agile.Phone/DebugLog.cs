namespace TecX.Agile.Phone
{
    using System;
    using System.Diagnostics;

    using Caliburn.Micro;

    public class DebugLog : ILog
    {
        private readonly Type type;

        public DebugLog(Type type)
        {
            Guard.AssertNotNull(type, "type");

            this.type = type;
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

        private static string CreateLogMessage(string format, params object[] args)
        {
            string message = string.Empty;
            if (args.Length == 1)
            {
                message = string.Format("[{0} - {1}]", string.Format(format, args[0]), DateTime.Now.ToString("T"));
            }
            else if (args.Length == 2)
            {
                message = string.Format(
                    "[{0} - {1}] {2}",
                    args[1],
                    DateTime.Now.ToString("T"),
                    string.Format(format, ((object[])args[0])));
            }

            return message;
        }
    }
}