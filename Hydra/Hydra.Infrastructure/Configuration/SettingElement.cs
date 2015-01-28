namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Reflection;

    [TypeConverter(typeof(SettingElementTypeConverter))]
    public class SettingElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty SettingNameProperty;

        private static readonly ConfigurationProperty ValueProperty;

        private static readonly ConfigurationProperty TypeProperty;

        private static readonly ConfigurationPropertyCollection ConfigProperties;

        static SettingElement()
        {
            SettingNameProperty = new ConfigurationProperty(
                "name",
                typeof(SettingName),
                SettingName.Empty,
                new SettingNameTypeConverter(),
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

        protected override ConfigurationPropertyCollection Properties
        {
            get { return ConfigProperties; }
        }
    }
}