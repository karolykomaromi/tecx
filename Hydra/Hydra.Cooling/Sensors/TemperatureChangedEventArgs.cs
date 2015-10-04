namespace Hydra.Cooling.Sensors
{
    using System.Diagnostics.Contracts;

    public class TemperatureChangedEventArgs : SensorStateChangedEventArgs
    {
        private readonly Temperature newTemperature;

        public TemperatureChangedEventArgs(IDevice device, Temperature newTemperature)
            : base(device)
        {
            Contract.Requires(newTemperature != null);

            this.newTemperature = newTemperature;
        }

        public Temperature NewTemperature
        {
            get { return this.newTemperature; }
        }
    }
}