namespace Hydra.Cooling
{
    public abstract class Thermostat : Device, IThermostat
    {
        public static new readonly IThermostat Null = new NullThermostat();

        public abstract void SetTargetTemperature(Temperature temperature);

        private class NullThermostat : NullDevice, IThermostat
        {
            public void SetTargetTemperature(Temperature temperature)
            {
            }
        }
    }
}