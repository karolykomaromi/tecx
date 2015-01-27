namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class PersistentSettingTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Setting);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            PersistentSetting ps = value as PersistentSetting;

            if (ps == null)
            {
                return Setting.Empty;
            }

            SettingName sn;
            if (!SettingName.TryParse(ps.Name, out sn))
            {
                return Setting.Empty;
            }

            object o;
            using (Stream stream = new MemoryStream(ps.Value ?? new byte[0]))
            {
                o = new BinaryFormatter().Deserialize(stream);
            }

            Setting setting = new Setting(sn, o);

            return setting;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (Setting);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Setting s = value as Setting;

            if (s == null)
            {
                return null;
            }

            byte[] blob;

            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, s.Value);

                blob = stream.GetBuffer();
            }

            PersistentSetting ps = new PersistentSetting
            {
                Name = s.Name.ToString(),
                Value = blob
            };

            return ps;
        }
    }
}