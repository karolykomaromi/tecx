using System.Diagnostics.Contracts;
using Cars.Measures;

namespace Cars.Parts.Engines
{
    public class FuelConsumptionBuilder : Builder<FuelConsumption>
    {
        private decimal liters;

        public FuelConsumptionBuilder()
        {
            this.liters = 0;
        }

        public FuelConsumptionBuilder Liters(decimal liters)
        {
            Contract.Ensures(Contract.Result<FuelConsumptionBuilder>() != null);

            this.liters = liters;

            return this;
        }

        public FuelConsumptionBuilder Liters(double liters)
        {
            Contract.Ensures(Contract.Result<FuelConsumptionBuilder>() != null);

            this.liters = new decimal(liters);

            return this;
        }

        public override FuelConsumption Build()
        {
            return new FuelConsumption
            {
                Distance = Distance.FromKilometers(100),
                FuelType = FuelTypes.Diesel,
                Volume = Volume.FromLiters(this.liters)
            };
        }
    }
}