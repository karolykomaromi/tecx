namespace Infrastructure.Options
{
    using System;
    using System.Globalization;

    public class UnknownOptionException : Exception
    {
        public UnknownOptionException(Option option, Type optionType)
            : base(string.Format(CultureInfo.CurrentCulture, "You tried to access option '{0}' which is not known to options of Type '{1}'.", optionType.FullName, option))
        {
        }
    }
}