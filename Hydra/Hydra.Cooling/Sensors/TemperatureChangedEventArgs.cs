namespace Hydra.Cooling.Sensors
{
    using System.Diagnostics.Contracts;

    public class TemperatureChangedEventArgs : DeviceStateChangedEventArgs<ITemperatureSensor>
    {
        private readonly Temperature oldTemperature;

        private readonly Temperature newTemperature;

        public TemperatureChangedEventArgs(ITemperatureSensor device, Temperature oldTemperature, Temperature newTemperature)
            : base(device)
        {
            Contract.Requires(newTemperature != null);
            Contract.Requires(oldTemperature != null);

            this.oldTemperature = oldTemperature;
            this.newTemperature = newTemperature;
        }

        public Temperature NewTemperature
        {
            get { return this.newTemperature; }
        }

        public Temperature OldTemperature
        {
            get { return this.oldTemperature; }
        }
    }
}