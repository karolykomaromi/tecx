namespace TecX.EnumClasses
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class EnumClassConverter<T> : TypeConverter where T : Enumeration
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.IsAssignableFrom(typeof(T));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string name = (string)value;

            return Enumeration.FromName<T>(name);
        }
    }
}