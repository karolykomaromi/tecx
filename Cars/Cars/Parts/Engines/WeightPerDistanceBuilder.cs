namespace Cars.Parts.Engines
{
    using System.Diagnostics.Contracts;
    using Cars.Measures;

    public class WeightPerDistanceBuilder : Builder<WeightPerDistance>
    {
        private Weight weight;

        public WeightPerDistanceBuilder()
        {
            this.weight = Measures.Weight.Zero;
        }

        public WeightPerDistanceBuilder Weight(Weight weight)
        {
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