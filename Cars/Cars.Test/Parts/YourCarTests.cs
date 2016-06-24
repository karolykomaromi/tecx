namespace Cars.Test.Parts
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Cars.Financial;
    using Cars.Parts;

    public class YourCarTests
    {
    }

    public class YourCar
    {
        private readonly Model model;

        private readonly PartCollection selectedParts;

        public YourCar(Model model)
        {
            this.model = model;
            this.selectedParts = new PartCollection();
        }

        public CurrencyAmount Total
        {
            get { return this.selectedParts.Select(p => p.Price).Sum(); }
        }

        public PartCollection SelectedParts
        {
            get { return this.selectedParts; }
        }
    }

    public class Model
    {
        private readonly PartNumber partNumber;

        private readonly PartCollection availableParts;

        public Model(PartNumber partNumber, params Part[] availableParts)
        {
            Contract.Requires(partNumber != null);

            this.partNumber = partNumber;
            this.availableParts = new PartCollection();

            foreach (Part part in availableParts ?? new Part[0])
            {
                this.AvailableParts.AddNewAndReplaceExisting(part);
            }
        }

        public PartCollection AvailableParts
        {
            get
            {
                Contract.Ensures(Contract.Result<PartCollection>() != null);

                return this.availableParts;
            }
        }
    }
}