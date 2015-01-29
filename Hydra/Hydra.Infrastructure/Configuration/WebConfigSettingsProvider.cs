namespace Hydra.Infrastructure.Configuration
{
    using System.Web.Configuration;

    public class WebConfigSettingsProvider : ConfigFileSettingsProvider
    {
        public WebConfigSettingsProvider()
            : base(WebConfigurationManager.OpenWebConfiguration("/"))
        {
        }
    }
}