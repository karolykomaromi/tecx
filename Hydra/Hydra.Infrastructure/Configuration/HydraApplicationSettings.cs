namespace Hydra.Infrastructure.Configuration
{
    using System.Configuration;

    public class HydraApplicationSettings : ConfigurationSection
    {
        [ConfigurationProperty("supportedLanguages", IsRequired = true)]
        public SupportedLanguagesCollection SupportedLanguages
        {
            get { return (SupportedLanguagesCollection)base["supportedLanguages"]; }
        }
    }
}