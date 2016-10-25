namespace Cars.Parts.Engines
{
    using System.Diagnostics.Contracts;
    using Cars.I18n;

    public class FuelType
    {
        private readonly PolyglotString name;

        public FuelType(PolyglotString name)
        {
            Contract.Requires(name != null);

            this.name = name;
        }

        public PolyglotString Name
        {
            get
            {
                Contract.Ensures(Contract.Result<PolyglotString>() != null);

                return this.name;
            }
        }
    }
}