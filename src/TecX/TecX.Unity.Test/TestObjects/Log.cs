namespace TecX.Unity.Test.TestObjects
{
    using System;

    public class Log : ILog
    {
        public Type Type { get; set; }

        public Log(Type type)
        {
            Type = type;
        }
    }
}