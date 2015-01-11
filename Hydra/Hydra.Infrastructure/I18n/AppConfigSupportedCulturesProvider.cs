namespace Hydra.Infrastructure.I18n
{
    using System.Configuration;

    public class AppConfigSupportedCulturesProvider : ConfigFileSupportedCulturesProvider
    {
        public AppConfigSupportedCulturesProvider()
            : base(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal))
        {   
        }
    }
}