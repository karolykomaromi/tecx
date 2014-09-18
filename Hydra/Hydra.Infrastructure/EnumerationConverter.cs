namespace Hydra.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public class EnumerationConverter<T> : TypeConverter where T : Enumeration<T>
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(int))
            {
                return true;
            }

            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return true;
            }

            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is int)
            {
                T converted = Enumeration<T>.GetValues().FirstOrDefault(v => v.Value == (int)value);

                return converted ?? Enumeration<T>.Default;
            }

            string name = value as string;
            if (!string.IsNullOrEmpty(name))
            {
                T converted = Enumeration<T>.GetValues().FirstOrDefault(v => string.Equals(v.Name, name, StringComparison.OrdinalIgnoreCase));

                return converted ?? Enumeration<T>.Default;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return ((T)value).Value;
            }

            if (destinationType == typeof(string))
            {
                return ((T)value).Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}