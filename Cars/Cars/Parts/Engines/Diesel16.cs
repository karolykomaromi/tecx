namespace Cars.Parts.Engines
{
    using Cars.Measures;

    public class Diesel16 : EngineBuilder
    {
        public Diesel16()
        {
            this.PartNumber("DIESEL_1.6")
                .CylinderCapacity(Volume.FromCubicCentimeters(1560))
                .MaxPower(x => x.Power(Power.FromKiloWatts(85)).AtRpm(3750))
                .MaxTorque(x => x.Torque(Torque.FromNewtonMeter(300)).AtRpm(1750))
                .Transmission(x => x.NumberOfGears(6).Type(TransmissionTypes.Stickshift))
                .MinAverageFuelConsumption(x => x.Liters(5.1))
                .MaxAverageFuelConsumption(x => x.Liters(5.2))
                .MaxExhaust(x => x.Weight(Weight.FromGrams(137)))
                .MinExhaust(x => x.Weight(Weight.FromGrams(133)));
        }
    }
}