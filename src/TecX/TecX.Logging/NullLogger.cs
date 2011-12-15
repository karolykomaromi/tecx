namespace TecX.Logging
{
    using System;

    public class NullLogger : ILogger
    {
        public void Verbose(string message)
        {
        }

        public void Verbose(string format, params object[] args)
        {
        }

        public void Info(string message)
        {
        }

        public void Info(string format, params object[] args)
        {
        }

        public void Warning(string message)
        {
        }

        public void Warning(string format, params object[] args)
        {
        }

        public void Error(Exception exception)
        {
        }

        public void Error(Exception exception, string message)
        {
        }

        public void Error(Exception exception, string format, params object[] args)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string format, params object[] args)
        {
        }
    }
}
