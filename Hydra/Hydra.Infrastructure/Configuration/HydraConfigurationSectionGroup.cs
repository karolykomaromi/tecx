namespace Hydra.Infrastructure.Configuration
{
    using System.Configuration;
    using System.Diagnostics.Contracts;

    public class HydraConfigurationSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("hydra.settings", IsRequired = true)]
        public HydraApplicationSettings ApplicationSettings
        {
            get { return (HydraApplicationSettings)this.Sections["hydra.settings"]; }
        }

        public static HydraConfigurationSectionGroup HydraConfiguration(Configuration configuration)
        {
            Contract.Requires(configuration != null);

            return (HydraConfigurationSectionGroup)configuration.GetSectionGroup("hydra");
        }
    }
}