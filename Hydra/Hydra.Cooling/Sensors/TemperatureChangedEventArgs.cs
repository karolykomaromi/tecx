namespace Hydra.Cooling.Sensors
{
    using System.Diagnostics.Contracts;

    public class TemperatureChangedEventArgs : DeviceStateChangedEventArgs<ITemperatureSensor>
    {
        private readonly Temperature newTemperature;

        public TemperatureChangedEventArgs(ITemperatureSensor device, Temperature newTemperature)
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