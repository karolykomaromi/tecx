namespace TecX.Common.Error
{
    using System;
    using System.Runtime.Serialization;

    public class ArgumentOutOfRangeException : System.ArgumentOutOfRangeException
    {
        public ArgumentOutOfRangeException()
        {
        }

        public ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentOutOfRangeException(string paramName)
            : base(paramName)
        {
        }

        public ArgumentOutOfRangeException(string paramName, object actualValue, string message)
            : base(paramName, actualValue, message)
        {
        }

        public ArgumentOutOfRangeException(string paramName, string message)
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