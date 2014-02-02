namespace Infrastructure.Options
{
    using System;

    public class UnknownOptionException : Exception
    {
        public UnknownOptionException(Option option, Type optionType)
            : base(string.Format("You tried to access option '{0}' which is not known to options of Type '{1}'.", optionType.FullName, option))
        {
        }
    }
}