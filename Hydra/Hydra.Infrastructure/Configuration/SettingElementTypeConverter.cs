namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class SettingElementTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Setting);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Setting s = value as Setting;

            if (s == null)
            {
                return null;
            }

            Type type = s.Value != null ? s.Value.GetType() : typeof(Missing);

            string v = SerializeValue(s.Value, type, culture);

            SettingElement se = new SettingElement
            {
                SettingName = s.Name,
                Type = type,
                Value = v
            };

            return se;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Setting);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SettingElement se = value as SettingElement;

            if (se == null)
            {
                return Setting.Empty;
            }

            object v = DeserializeValue(se.Value, se.Type, culture);

            Setting setting = new Setting(se.SettingName, v);

            return setting;
        }

        private static object DeserializeValue(string value, Type type, CultureInfo culture)
        {
            if (value == null || type == typeof(Missing))
            {
                return null;
            }

            if (type.IsPrimitive)
            {
                return ConvertHelper.Convert(value, type, culture);
            }

            if (type == typeof(string))
            {
                return value;
            }

            XmlSerializer serializer = new XmlSerializer(type);

            object o;
            using (TextReader reader = new StringReader(value))
            {
                o = serializer.Deserialize(reader);
            }

            return o;
        }

        private static string SerializeValue(object value, Type type, CultureInfo culture)
        {
            if (value == null || type == typeof(Missing))
            {
                return string.Empty;
            }

            if (type.IsPrimitive || type == typeof(string))
            {
                return (string)ConvertHelper.Convert(value, typeof(string), culture);
            }

            XmlSerializer serializer = new XmlSerializer(type);

            XmlWriterSettings xws = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true
                };

            StringBuilder sb = new StringBuilder(100);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ns.Add(string.Empty, string.Empty);

            using (TextWriter writer = new StringWriter(sb, culture))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, xws))
                {
                    serializer.Serialize(xmlWriter, value, ns);
                }
            }

            return sb.ToString();
        }
    }
}