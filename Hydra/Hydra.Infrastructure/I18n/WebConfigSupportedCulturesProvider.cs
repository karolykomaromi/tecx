namespace Hydra.Infrastructure.I18n
{
    using System.Web.Configuration;

    public class WebConfigSupportedCulturesProvider : ConfigFileSupportedCulturesProvider
    {
        public WebConfigSupportedCulturesProvider()
            : base(WebConfigurationManager.OpenWebConfiguration("/"))
        {   
        }
    }
}