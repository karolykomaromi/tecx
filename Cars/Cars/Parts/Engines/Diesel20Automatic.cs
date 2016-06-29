using Cars.Measures;

namespace Cars.Parts.Engines
{
    public class Diesel20Automatic : EngineBuilder
    {
        public Diesel20Automatic()
        {
            this.PartNumber("DIESEL_2.0_AUTOMATIC")
                .CylinderCapacity(Volume.FromCubicCentimeters(1997))
                .MaxPower(x => x.Power(Power.FromKiloWatts(132)).AtRpm(4000))
                .MaxTorque(x => x.Torque(Torque.FromNewtonMeter(4000)).AtRpm(2000))
                .Transmission(x => x.NumberOfGears(6).Type(TransmissionTypes.Automatic))
                .MinAverageFuelConsumption(x => x.Liters(5.7))
                .MaxAverageFuelConsumption(x => x.Liters(6.0))
                .MinExhaust(x => x.Weight(Weight.FromGrams(151)))
                .MaxExhaust(x => x.Weight(Weight.FromGrams(159)))
                .ExhaustEfficiencyClass(ExhaustEfficiencyClasses.C);
        }
    }
}