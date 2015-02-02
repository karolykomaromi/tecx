namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class SettingNameTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SettingName settingName = value as SettingName;

            if (settingName == null)
            {
                return string.Empty;
            }

            return settingName.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            SettingName sn;
            if (SettingName.TryParse(value as string, out sn))
            {
                return sn;
            }

            return SettingName.Empty;
        }
    }
}