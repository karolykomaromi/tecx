namespace Hydra.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Count={Count}")]
    public class CompositeSettingsProvider : ISettingsProvider
    {
        private readonly List<ISettingsProvider> providers;

        public CompositeSettingsProvider(params ISettingsProvider[] providers)
        {
            this.providers = new List<ISettingsProvider>();

            this.AddRange(providers);
        }

        public int Count
        {
            get { return this.providers.Count; }
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

        private void AddRange(IEnumerable<ISettingsProvider> providers)
        {
            if (providers == null)
            {
                return;
            }

            foreach (var provider in providers)
            {
                if (object.ReferenceEquals(this, provider))
                {
                    continue;
                }

                CompositeSettingsProvider other = provider as CompositeSettingsProvider;

                if (other != null)
                {
                    this.AddRange(other.providers);
                }
                else if (!this.providers.Contains(provider))
                {
                    this.providers.Add(provider);
                }
            }
        }
    }
}