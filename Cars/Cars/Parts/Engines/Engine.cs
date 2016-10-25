namespace Cars.Parts.Engines
{
    using Cars.Measures;

    public class Engine : Part
    {
        public Engine(PartNumber partNumber)
            : base(partNumber)
        {
        }

        public Engine(PartNumber partNumber, PartNumber[] replacesTheseParts, PartNumber[] cantBeCombinedWithTheseParts)
            : base(partNumber, replacesTheseParts, cantBeCombinedWithTheseParts)
        {
        }

        public Volume CylinderCapacity { get; set; }

        public bool HasStartStopSystem { get; set; }

        public PowerAtRpm MaxPower { get; set; }

        public TorqueAtRpm MaxTorque { get; set; }

        public Transmission Transmission { get; set; }

        public FuelConsumption MinAverageFuelConsumption { get; set; }

        public FuelConsumption MaxAverageFuelConsumption { get; set; }

        public ExhaustEmissionStandard ExhaustEmissionStandard { get; set; }
    }
}