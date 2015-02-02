namespace Hydra.Infrastructure.Configuration
{
    using System.Collections.Generic;

    public class CompositeSettingsProvider : ISettingsProvider
    {
        private readonly List<ISettingsProvider> providers;

        public CompositeSettingsProvider(params ISettingsProvider[] providers)
        {
            this.providers = new List<ISettingsProvider>(providers ?? new ISettingsProvider[0]);
        }

        public SettingsCollection GetSettings()
        {
            if (this.providers.Count == 0)
            {
                return new SettingsCollection();
            }

            if (this.providers.Count == 1)
            {
                return this.providers[0].GetSettings();
            }

            SettingsCollection sc = this.providers[0].GetSettings();

            for (int i = 1; i < this.providers.Count; i++)
            {
                sc = sc.Merge(this.providers[i].GetSettings());
            }

            return sc;
        }
    }
}