namespace Hydra.Configuration
{
    using System.Configuration;
    using System.Diagnostics.Contracts;

    public class HydraConfigurationSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("applicationSettings", IsRequired = true)]
        public HydraApplicationSettings ApplicationSettings
        {
            get { return (HydraApplicationSettings)this.Sections["applicationSettings"]; }
        }

        public static HydraConfigurationSectionGroup HydraConfiguration(Configuration configuration)
        {
            Contract.Requires(configuration != null);

            return (HydraConfigurationSectionGroup)configuration.GetSectionGroup("hydra");
        }
    }
}