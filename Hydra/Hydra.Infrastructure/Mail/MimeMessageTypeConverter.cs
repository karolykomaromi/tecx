namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using MimeKit;

    public class MimeMessageTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string s = value as string;

            if (s == null)
            {
                return value;
            }

            return MimeMessageBuilder.FromString(s).Build();
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            MimeMessage message = value as MimeMessage;

            if (message == null)
            {
                return value;
            }

            return MimeMessageBuilder.ToString(message);
        }
    }
}