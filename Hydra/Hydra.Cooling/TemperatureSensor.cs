namespace Hydra.Cooling
{
    public abstract class TemperatureSensor : Device, ITemperatureSensor
    {
        public static new readonly ITemperatureSensor Null = new NullSensor();

        public abstract Temperature CurrentTemperature { get; }

        private class NullSensor : NullDevice, ITemperatureSensor
        {
            public Temperature CurrentTemperature
            {
                get { return Temperature.Invalid; }
            }
        }
    }
}