namespace Hydra.Infrastructure.Configuration
{
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;

    public class SupportedLanguageElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty LanguageProperty;

        private static readonly ConfigurationPropertyCollection ConfigProperties;

        static SupportedLanguageElement()
        {
            LanguageProperty = new ConfigurationProperty(
                "lang",
                typeof(CultureInfo),
                CultureInfo.InvariantCulture,
                new CultureInfoConverter(),
                null,
                ConfigurationPropertyOptions.None);

            ConfigProperties = new ConfigurationPropertyCollection
            {
                LanguageProperty
            };
        }

        public CultureInfo Culture
        {
            get { return (CultureInfo)base[LanguageProperty]; }

            set { base[LanguageProperty] = value; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return ConfigProperties; }
        }
    }
}