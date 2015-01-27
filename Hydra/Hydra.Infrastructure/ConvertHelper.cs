namespace Hydra.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Hydra.Infrastructure.Logging;

    public static class ConvertHelper
    {
        public static bool TryConvert(object o, Type destinationType, CultureInfo culture, out object converted)
        {
            if (o != null)
            {
                if (destinationType.IsInstanceOfType(o))
                {
                    converted = o;
                    return true;
                }

                TypeConverter converter1 = TypeDescriptor.GetConverter(destinationType);

                if (converter1.CanConvertFrom(o.GetType()))
                {
                    try
                    {
                        converted = converter1.ConvertFrom(null, culture, o);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                        converted = null;
                        return false;
                    }
                }

                TypeConverter converter2 = TypeDescriptor.GetConverter(o);

                if (converter2.CanConvertTo(destinationType))
                {
                    try
                    {
                        converted = converter2.ConvertTo(null, culture, o, destinationType);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                        converted = null;
                        return false;
                    }
                }

                if (destinationType == typeof(Type))
                {
                    converted = Type.GetType(o.ToString(), true);
                    return true;
                }

                converted = null;
                return false;
            }

            if (destinationType.IsValueType)
            {
                // equivalent to using default(MyType)
                converted = Activator.CreateInstance(destinationType);
                return true;
            }

            converted = null;
            return false;
        }

        public static object Convert(object o, Type destinationType, CultureInfo culture)
        {
            object converted;
            if (!ConvertHelper.TryConvert(o, destinationType, culture, out converted))
            {
                string msg = string.Format(Properties.Resources.ConversionNotSupported, o, destinationType.AssemblyQualifiedName);
                throw new NotSupportedException(msg);
            }

            return converted;
        }

        public static bool TryConvertInvariant(object o, Type destinationType, out object converted)
        {
            return ConvertHelper.TryConvert(o, destinationType, CultureInfo.InvariantCulture, out converted);
        }

        public static object ConvertInvariant(object o, Type destinationType)
        {
            return ConvertHelper.Convert(o, destinationType, CultureInfo.InvariantCulture);
        }
    }
}
