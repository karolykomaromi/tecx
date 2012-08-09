namespace TecX.Unity.Test.TestObjects
{
    using System;

    public static class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            return new Log(type);
        }
    }
}