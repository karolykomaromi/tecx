namespace Cars.Parts.Engines
{
    using Cars.Measures;

    public class FuelConsumption
    {
        public FuelType FuelType { get; set; }

        public Distance Distance { get; set; }

        public Volume Volume { get; set; }

        public override string ToString()
        {
            return string.Format(Cars.Properties.Resources.FuelConsumptionPerDistance, this.Volume, this.FuelType, this.Distance);
        }
    }
}