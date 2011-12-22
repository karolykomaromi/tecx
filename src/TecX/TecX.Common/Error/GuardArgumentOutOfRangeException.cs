namespace TecX.Common.Error
{
    using System;
    using System.Runtime.Serialization;

    public class GuardArgumentOutOfRangeException : ArgumentOutOfRangeException
    {
        public GuardArgumentOutOfRangeException()
        {
        }

        public GuardArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GuardArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public GuardArgumentOutOfRangeException(string paramName)
            : base(paramName)
        {
        }

        public GuardArgumentOutOfRangeException(string paramName, object actualValue, string message)
            : base(paramName, actualValue, message)
        {
        }

        public GuardArgumentOutOfRangeException(string paramName, string message)
            : base(paramName, message)
        {
        }

        public override string StackTrace
        {
            get
            {
                return StackTraceCleaner.Clean(base.StackTrace);
            }
        }
    }
}