namespace Hydra.Cooling.Actuators
{
    using System.Diagnostics.Contracts;
    using Hydra.Cooling.Sensors;

    public class ThermostatTargetTemperatureChangedEventArgs : DeviceStateChangedEventArgs<IThermostat>
    {
        private readonly Temperature oldTargetTemperature;

        private readonly Temperature newTargetTemperature;

        public ThermostatTargetTemperatureChangedEventArgs(IThermostat device, Temperature oldTargetTemperature, Temperature newTargetTemperature)
            : base(device)
        {
            Contract.Requires(oldTargetTemperature != null);
            Contract.Requires(newTargetTemperature != null);

            this.oldTargetTemperature = oldTargetTemperature;
            this.newTargetTemperature = newTargetTemperature;
        }

        public Temperature NewTargetTemperature
        {
            get { return this.newTargetTemperature; }
        }

        public Temperature OldTargetTemperature
        {
            get
            {
                return this.oldTargetTemperature;
            }
        }
    }
}