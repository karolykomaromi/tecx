namespace Hydra.Configuration
{
    using System.Configuration;

    public class HydraConfigurationSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("applicationSettings", IsRequired = true)]
        public HydraApplicationSettings ApplicationSettings
        {
            get { return (HydraApplicationSettings)this.Sections["applicationSettings"]; }
        }
    }
}