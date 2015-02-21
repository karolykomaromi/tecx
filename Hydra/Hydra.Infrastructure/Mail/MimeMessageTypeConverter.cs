namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
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

            using (Stream stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.Write(s);
                    writer.Flush();

                    stream.Position = 0;

                    MimeMessage message = MimeMessage.Load(stream);

                    return message;
                }
            }
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

            using (Stream stream = new MemoryStream())
            {
                message.WriteTo(stream);

                stream.Position = 0;

                using (TextReader reader = new StreamReader(stream))
                {
                    string s = reader.ReadToEnd();

                    return s;
                }
            }
        }
    }
}