namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public abstract class SupportedCulturesProvider
    {
        private static SupportedCulturesProvider current = new InMemorySupportedCulturesProvider(Cultures.EnglishUnitedStates, Cultures.GermanGermany);

        public static SupportedCulturesProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                Contract.Requires(value != null);

                current = value;
            }
        }

        public static CultureInfo[] SupportedCultures
        {
            get { return Current.GetSupportedCultures(); }
        }

        protected internal abstract CultureInfo[] GetSupportedCultures();
    }
}