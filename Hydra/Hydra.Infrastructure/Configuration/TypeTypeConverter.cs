namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    public class TypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string typeName = value as string;

            if (string.IsNullOrWhiteSpace(typeName))
            {
                return typeof(Missing);
            }

            Type type = Type.GetType(typeName, false);

            if (type != null)
            {
                return type;
            }

            return typeof(Missing);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Type type = value as Type;

            if (type != null)
            {
                return type.AssemblyQualifiedName;
            }

            return typeof(Missing).AssemblyQualifiedName;
        }
    }
}