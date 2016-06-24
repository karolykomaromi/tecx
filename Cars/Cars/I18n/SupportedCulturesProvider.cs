namespace Cars.I18n
{
    using System.Collections.Generic;
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

        public static IReadOnlyList<CultureInfo> SupportedCultures
        {
            get { return Current.GetSupportedCultures(); }
        }

        protected internal virtual IReadOnlyList<CultureInfo> GetSupportedCultures()
        {
            Contract.Ensures(Contract.Result<IReadOnlyList<CultureInfo>>() != null);

            return new CultureInfo[0];
        }
    }
}