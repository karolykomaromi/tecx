namespace Cars.Parts.Engines
{
    using Cars.Measures;

    public class Diesel20Manual : EngineBuilder
    {
        public Diesel20Manual()
        {
            this.PartNumber("DIESEL_2.0_MANUAL")
                .CylinderCapacity(Volume.FromCubicCentimeters(1997))
                .MaxPower(x => x.Power(Power.FromKiloWatts(110)).AtRpm(4000))
                .MaxTorque(x => x.Torque(Torque.FromNewtonMeter(370)).AtRpm(2000))
                .Transmission(x => x.NumberOfGears(6).Type(TransmissionTypes.Stickshift))
                .MinAverageFuelConsumption(x => x.Liters(5.3))
                .MaxAverageFuelConsumption(x => x.Liters(5.5))
                .MaxExhaust(x => x.Weight(Weight.FromGrams(147)))
                .MinExhaust(x => x.Weight(Weight.FromGrams(139)));
        }
    }
}