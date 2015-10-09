namespace Hydra.Cooling
{
    public class KelvinEpsilonComparer : TemperatureEpsilonComparer<Kelvin>
    {
        public KelvinEpsilonComparer(Kelvin epsilon)
            : base(epsilon)
        {
        }
    }
}