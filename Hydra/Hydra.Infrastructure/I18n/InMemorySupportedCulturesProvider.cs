namespace Hydra.Infrastructure.I18n
{
    using System.Globalization;

    public class InMemorySupportedCulturesProvider : SupportedCulturesProvider
    {
        private readonly CultureInfo[] cultures;

        public InMemorySupportedCulturesProvider(params CultureInfo[] cultures)
        {
            this.cultures = cultures ?? new CultureInfo[0];
        }

        protected internal override CultureInfo[] GetSupportedCultures()
        {
            return this.cultures;
        }
    }
}