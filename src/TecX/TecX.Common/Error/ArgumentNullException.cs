namespace TecX.Common.Error
{
    using System;
    using System.Runtime.Serialization;

    public class ArgumentNullException : System.ArgumentNullException
    {
        public ArgumentNullException()
        {
        }

        public ArgumentNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentNullException(string paramName)
            : base(paramName)
        {
        }

        public ArgumentNullException(string paramName, string message)
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
