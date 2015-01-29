namespace Hydra.Infrastructure.Configuration
{
    using System.Configuration;

    public class AppConfigSettingsProvider : ConfigFileSettingsProvider
    {
        public AppConfigSettingsProvider()
            : base(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal))
        {
        }
    }
}