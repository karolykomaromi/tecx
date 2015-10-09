namespace Hydra.Cooling
{
    public class FahrenheitEpsilonComparer : TemperatureEpsilonComparer<Fahrenheit>
    {
        public FahrenheitEpsilonComparer(Fahrenheit epsilon)
            : base(epsilon)
        {
        }
    }
}