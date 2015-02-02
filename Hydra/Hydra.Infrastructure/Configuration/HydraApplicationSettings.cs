namespace Hydra.Infrastructure.Configuration
{
    using System.Configuration;

    public class HydraApplicationSettings : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SettingElementCollection Settings
        {
            get { return (SettingElementCollection)base[string.Empty]; }
        }

        [ConfigurationProperty("supportedLanguages", IsRequired = true)]
        public SupportedLanguageElementCollection SupportedLanguages
        {
            get { return (SupportedLanguageElementCollection)base["supportedLanguages"]; }
        }
    }
}