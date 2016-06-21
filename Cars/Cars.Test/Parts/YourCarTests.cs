using System.Linq;
using Cars.Financial;
using Cars.Parts;

namespace Cars.Test.Parts
{
    public class YourCarTests
    {
    }

    public class YourCar
    {
        private readonly PartCollection selectedParts;

        public YourCar()
        {
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
}