using System.Diagnostics.Contracts;
using Cars.Measures;

namespace Cars.Parts.Engines
{
    public class WeightPerDistanceBuilder : Builder<WeightPerDistance>
    {
        private Weight weight;

        public WeightPerDistanceBuilder()
        {
            this.weight = Measures.Weight.Zero;
        }

        public WeightPerDistanceBuilder Weight(Weight weight)
        {
            Contract.Requires(weight != null);
            Contract.Ensures(Contract.Result<WeightPerDistanceBuilder>() != null);

            this.weight = weight;

            return this;
        }

        public override WeightPerDistance Build()
        {
            return new WeightPerDistance
            {
                Weight = this.weight,
                Distance = Distance.FromKilometers(100)
            };
        }
    }
}