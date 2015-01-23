namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class SupportedLanguagesCollection : ConfigurationElementCollection, IEnumerable<SupportedLanguage>
    {
        IEnumerator<SupportedLanguage> IEnumerable<SupportedLanguage>.GetEnumerator()
        {
            return this.OfType<SupportedLanguage>().GetEnumerator();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedLanguage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SupportedLanguage)element).Culture.ToString();
        }
    }

    public class SettingsElementCollection : ConfigurationElementCollection, IEnumerable<SettingElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SettingElement)element).SettingName;
        }

        public IEnumerator<SettingElement> GetEnumerator()
        {
            return this.OfType<SettingElement>().GetEnumerator();
        }
    }

    public class SettingElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty SettingNameProperty;

        private static readonly ConfigurationProperty ValueProperty;

        private static readonly ConfigurationProperty TypeProperty;

        private static readonly ConfigurationPropertyCollection ConfigProperties;

        static SettingElement()
        {
            SettingNameProperty = new ConfigurationProperty(
                "lang",
                typeof(CultureInfo),
                CultureInfo.InvariantCulture,
                new CultureInfoConverter(),
                null,
                ConfigurationPropertyOptions.IsRequired);

            ValueProperty = new ConfigurationProperty(
                "value",
                typeof(string),
                string.Empty,
                null,
                null,
                ConfigurationPropertyOptions.IsRequired);

            TypeProperty = new ConfigurationProperty(
                "type",
                typeof(Type),
                typeof(Missing),
                new TypeTypeConverter(),
                null,
                ConfigurationPropertyOptions.IsRequired);

            ConfigProperties = new ConfigurationPropertyCollection
            {
                SettingNameProperty,
                ValueProperty,
                TypeProperty
            };
        }

        public SettingName SettingName
        {
            get { return (SettingName)base[SettingNameProperty]; }

            set { base[SettingNameProperty] = value; }
        }

        public string Value
        {
            get { return (string)base[ValueProperty]; }

            set { base[ValueProperty] = value; }
        }

        public Type Type
        {
            get { return (Type)base[TypeProperty]; }

            set { base[TypeProperty] = value; }
        }

        public Setting ToSetting()
        {
            return Setting.Empty;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return ConfigProperties; }
        }
    }

    public class SettingNameTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(SettingName);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SettingName sn;
            if (SettingName.TryParse(value as string, out sn))
            {
                return sn;
            }

            return SettingName.Empty;
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

    public class TypeTypeConverter : TypeConverter
    {
    }
}