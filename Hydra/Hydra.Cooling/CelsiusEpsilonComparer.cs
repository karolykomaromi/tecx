namespace Hydra.Cooling
{
    public class CelsiusEpsilonComparer : TemperatureEpsilonComparer<Celsius>
    {
        public CelsiusEpsilonComparer(Celsius epsilon)
            : base(epsilon)
        {
        }
    }
}