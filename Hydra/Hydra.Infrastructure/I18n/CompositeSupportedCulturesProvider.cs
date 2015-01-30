namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CompositeSupportedCulturesProvider : SupportedCulturesProvider
    {
        private readonly HashSet<SupportedCulturesProvider> providers;

        public CompositeSupportedCulturesProvider(params SupportedCulturesProvider[] providers)
        {
            this.providers = new HashSet<SupportedCulturesProvider>(providers ?? new SupportedCulturesProvider[0]);
        }

        protected internal override CultureInfo[] GetSupportedCultures()
        {
            var supportedCultures = this.providers
                .SelectMany(p => p.GetSupportedCultures())
                .OrderBy(ci => ci.Name)
                .ToArray();

            return supportedCultures;
        }
    }
}
