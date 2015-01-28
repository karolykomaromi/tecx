namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public abstract class SupportedCulturesProvider
    {
        private static SupportedCulturesProvider currentProvider = new InMemorySupportedCulturesProvider(Cultures.EnglishUnitedStates, Cultures.GermanGermany);

        public static SupportedCulturesProvider Current
        {
            get
            {
                return currentProvider;
            }

            set
            {
                Contract.Requires(value != null);

                currentProvider = value;
            }
        }

        public static CultureInfo[] SupportedCultures
        {
            get { return Current.GetSupportedCultures(); }
        }

        protected internal virtual CultureInfo[] GetSupportedCultures()
        {
            Contract.Ensures(Contract.Result<CultureInfo[]>() != null);

            return new CultureInfo[0];
        }
    }
}