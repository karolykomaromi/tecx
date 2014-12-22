namespace Hydra.Import
{
    using System;
    using System.Globalization;

    public abstract class ValueWriter : IValueWriter, IEquatable<IValueWriter>
    {
        public static readonly IValueWriter Null = new NullValueWriter();

        public abstract string PropertyName { get; }

        public abstract void Write(object instance, string value, CultureInfo source, CultureInfo target);

        public virtual bool Equals(IValueWriter other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.GetType() == other.GetType())
            {
                return true;
            }

            return false;
        }
    }
}