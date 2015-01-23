namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [ContractClass(typeof(SupportedCulturesProviderContract))]
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

    [ContractClassFor(typeof(SupportedCulturesProvider))]
    internal abstract class SupportedCulturesProviderContract : SupportedCulturesProvider
    {
        protected internal override CultureInfo[] GetSupportedCultures()
        {
            Contract.Ensures(Contract.Result<CultureInfo[]>() != null);

            return new CultureInfo[0];
        }
    }
}