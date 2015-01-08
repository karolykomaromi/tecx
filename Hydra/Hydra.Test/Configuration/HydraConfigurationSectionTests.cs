namespace Hydra.Test.Configuration
{
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using Hydra.Infrastructure.I18n;
    using Xunit;

    public class HydraConfigurationSectionTests
    {
        [Fact]
        public void Should_Read_Supported_Languages_From_Config_File()
        {
            HydraConfigurationSectionGroup group = (HydraConfigurationSectionGroup)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("hydra");

            Assert.NotNull(group);
            Assert.NotNull(group.ApplicationSettings);
            Assert.NotNull(group.ApplicationSettings.SupportedLanguages);
            Assert.Equal(1, group.ApplicationSettings.SupportedLanguages.Count);
            Assert.Equal(Cultures.GermanGermany, group.ApplicationSettings.SupportedLanguages.OfType<SupportedLanguage>().First().Culture);
        }
    }

    public class HydraConfigurationSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("applicationSettings", IsRequired = true)]
        public HydraApplicationSettings ApplicationSettings
        {
            get { return (HydraApplicationSettings)base.Sections["applicationSettings"]; }
        }
    }

    public class HydraApplicationSettings : ConfigurationSection
    {
        [ConfigurationProperty("supportedLanguages", IsRequired = true)]
        public SupportedLanguagesCollection SupportedLanguages
        {
            get { return (SupportedLanguagesCollection)base["supportedLanguages"]; }
        }
    }

    public class SupportedLanguagesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedLanguage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SupportedLanguage)element).Culture.ToString();
        }
    }

    public class SupportedLanguage : ConfigurationElement
    {
        private static readonly ConfigurationProperty languageProperty;

        private static readonly ConfigurationPropertyCollection properties;

        static SupportedLanguage()
        {
            languageProperty = new ConfigurationProperty(
                "lang",
                typeof (CultureInfo),
                CultureInfo.InvariantCulture,
                new CultureInfoConverter(),
                null,
                ConfigurationPropertyOptions.None);

            properties = new ConfigurationPropertyCollection
                {
                    languageProperty
                };
        }

        public CultureInfo Culture
        {
            get { return (CultureInfo) base[languageProperty]; }

            set { base[languageProperty] = value; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }
}
