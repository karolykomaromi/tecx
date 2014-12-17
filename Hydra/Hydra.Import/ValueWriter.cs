namespace Hydra.Import
{
    using System.Globalization;

    public abstract class ValueWriter : IValueWriter
    {
        public static readonly IValueWriter Null = new NullValueWriter();

        public abstract string PropertyName { get; }

        public abstract void Write(object instance, string value, CultureInfo source, CultureInfo target);
    }
}