namespace Hydra.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class ConfigFileSettingsProvider : ISettingsProvider
    {
        private readonly Configuration configuration;

        public ConfigFileSettingsProvider(Configuration configuration)
        {
            Contract.Requires(configuration != null);

            this.configuration = configuration;
        }

        public SettingsCollection GetSettings()
        {
            var config = HydraConfigurationSectionGroup.HydraConfiguration(this.configuration);

            SettingElementCollection elements = config.ApplicationSettings.Settings;

            if (elements != null)
            {
                List<Setting> settings = new List<Setting>();

                foreach (SettingElement element in elements)
                {
                    object converted;
                    Setting setting;
                    if (ConvertHelper.TryConvert(element, typeof(Setting), CultureInfo.CurrentCulture, out converted) &&
                        (setting = converted as Setting) != null)
                    {
                        settings.Add(setting);
                    }
                }

                SettingsCollection sc = new SettingsCollection(settings.ToArray());

                return sc;
            }

            return new SettingsCollection();
        }
    }
}
