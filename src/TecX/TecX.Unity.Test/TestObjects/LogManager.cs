namespace TecX.Unity.Test.TestObjects
{
    using System;

    public class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            return new Log(type);
        }

        public ILog GetLoggerX(Type type)
        {
            return new Log(type);
        }
    }
}