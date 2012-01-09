namespace TecX.Logging
{
    using System;

    public interface ILogger
    {
        void Verbose(string message);

        void Verbose(string format, params object[] args);

        void Info(string message);

        void Info(string format, params object[] args);

        void Warning(string message);

        void Warning(string format, params object[] args);

        void Error(Exception exception);

        void Error(Exception exception, string message);

        void Error(Exception exception, string format, params object[] args);

        void Error(string message);

        void Error(string format, params object[] args);
    }
}
