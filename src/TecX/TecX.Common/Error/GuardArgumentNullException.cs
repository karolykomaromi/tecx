namespace TecX.Common.Error
{
    using System;
    using System.Runtime.Serialization;

    public class GuardArgumentNullException : ArgumentNullException
    {
        public GuardArgumentNullException()
        {
        }

        public GuardArgumentNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GuardArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public GuardArgumentNullException(string paramName)
            : base(paramName)
        {
        }

        public GuardArgumentNullException(string paramName, string message)
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
