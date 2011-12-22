namespace TecX.Common.Error
{
    using System;
    using System.Runtime.Serialization;

    public class GuardArgumentException : ArgumentException
    {
        public GuardArgumentException()
        {
        }

        public GuardArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GuardArgumentException(string message)
            : base(message)
        {
        }

        public GuardArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public GuardArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public GuardArgumentException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
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