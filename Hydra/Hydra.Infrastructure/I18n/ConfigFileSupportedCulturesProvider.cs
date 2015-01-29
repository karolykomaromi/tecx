namespace Hydra.Infrastructure.I18n
{
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Hydra.Infrastructure.Configuration;

    public class ConfigFileSupportedCulturesProvider : SupportedCulturesProvider
    {
        private readonly Configuration configuration;

        public ConfigFileSupportedCulturesProvider(Configuration configuration)
        {
            Contract.Requires(configuration != null);

            this.configuration = configuration;
        }

        protected internal override CultureInfo[] GetSupportedCultures()
        {
            var config = HydraConfigurationSectionGroup.HydraConfiguration(this.configuration);

            return config.ApplicationSettings.SupportedLanguages.Select(sl => sl.Culture).ToArray();
        }
    }
}