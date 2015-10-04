namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    [DebuggerDisplay("Count={Count}")]
    public class CompositeSupportedCulturesProvider : SupportedCulturesProvider
    {
        private readonly HashSet<SupportedCulturesProvider> providers;

        public CompositeSupportedCulturesProvider(params SupportedCulturesProvider[] providers)
        {
            this.providers = new HashSet<SupportedCulturesProvider>();

            this.AddRange(providers);
        }

        public int Count
        {
            get { return this.providers.Count; }
        }

        protected internal override IReadOnlyList<CultureInfo> GetSupportedCultures()
        {
            var supportedCultures = this.providers
                .SelectMany(p => p.GetSupportedCultures())
                .Distinct()
                .OrderBy(ci => ci.Name)
                .ToArray();

            return supportedCultures;
        }

        private void AddRange(IEnumerable<SupportedCulturesProvider> providers)
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

                CompositeSupportedCulturesProvider other = provider as CompositeSupportedCulturesProvider;

                if (other != null)
                {
                    this.AddRange(other.providers);
                }
                else
                {
                    this.providers.Add(provider);
                }
            }
        }
    }
}
