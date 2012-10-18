namespace TecX.EnumClasses
{
    using System;

    public class EnumerationParseException : Exception
    {
        public EnumerationParseException(string message)
            : base(message)
        {
        }
    }
}