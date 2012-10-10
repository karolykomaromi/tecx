namespace TecX.Common.Error
{
    using System;
    using System.Runtime.Serialization;

    public class ArgumentException : System.ArgumentException
    {
        public ArgumentException()
        {
        }

        public ArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ArgumentException(string message)
            : base(message)
        {
        }

        public ArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public ArgumentException(string message, string paramName, Exception innerException)
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