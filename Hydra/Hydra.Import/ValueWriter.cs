namespace Hydra.Import
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;

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

    public class FloatValueWriter : PropertyValueWriter
    {
        public FloatValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            float f;
            if (float.TryParse(value, NumberStyles.Float, source, out f))
            {
                this.Property.SetValue(instance, f);
            }
        }
    }

    public class DoubleValueWriter : PropertyValueWriter
    {
        public DoubleValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            double d;
            if (double.TryParse(value, NumberStyles.Float, source, out d))
            {
                this.Property.SetValue(instance, d);
            }
        }
    }

    public class DecimalValueWriter : PropertyValueWriter
    {
        public DecimalValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            decimal d;
            if (decimal.TryParse(value, NumberStyles.Float, source, out d))
            {
                this.Property.SetValue(instance, d);
            }
        }
    }
}