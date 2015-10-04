namespace Hydra.Cooling.Actuators
{
    public abstract class Thermostat : Device, IThermostat
    {
        public static new readonly IThermostat Null = new NullThermostat();

        protected Thermostat(DeviceId id)
            : base(id)
        {
        }

        public abstract void SetTargetTemperature(Temperature temperature);

        private class NullThermostat : NullDevice, IThermostat
        {
            public void SetTargetTemperature(Temperature temperature)
            {
            }
        }
    }
}