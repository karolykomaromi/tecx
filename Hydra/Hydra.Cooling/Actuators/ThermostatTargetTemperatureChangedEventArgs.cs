namespace Hydra.Cooling.Actuators
{
    using System.Diagnostics.Contracts;
    using Hydra.Cooling.Sensors;

    public class ThermostatTargetTemperatureChangedEventArgs : DeviceStateChangedEventArgs<IThermostat>
    {
        private readonly Temperature newTargetTemperature;

        public ThermostatTargetTemperatureChangedEventArgs(IThermostat device, Temperature newTargetTemperature)
            : base(device)
        {
            Contract.Requires(newTargetTemperature != null);

            this.newTargetTemperature = newTargetTemperature;
        }

        public Temperature NewTargetTemperature
        {
            get { return this.newTargetTemperature; }
        }
    }
}