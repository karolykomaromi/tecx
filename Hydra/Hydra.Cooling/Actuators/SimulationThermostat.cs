namespace Hydra.Cooling.Actuators
{
    public class SimulationThermostat : Thermostat
    {
        private Temperature targetTemperature;

        public SimulationThermostat(DeviceId id)
            : base(id)
        {
            this.targetTemperature = Temperature.Invalid;
        }

        public override Temperature TargetTemperature
        {
            get
            {
                return this.targetTemperature;
            }

            set
            {
                Temperature oldTargetTemperature = this.targetTemperature;

                this.targetTemperature = value;

                this.OnTargetTemperatureChanged(oldTargetTemperature, value);
            }
        }
    }
}