namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class Thermostat : Device, IThermostat
    {
        public static new readonly IThermostat Null = new NullThermostat();

        protected Thermostat(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<ThermostatTargetTemperatureChangedEventArgs> TargetTemperatureChanged = delegate { };
        
        public abstract Temperature TargetTemperature { get; set; }

        protected virtual void OnTargetTemperatureChanged(ThermostatTargetTemperatureChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.TargetTemperatureChanged(this, args);
        }

        protected virtual void OnTargetTemperatureChanged(Temperature newTargetTemperature)
        {
            Contract.Requires(newTargetTemperature != null);

            this.TargetTemperatureChanged(this, new ThermostatTargetTemperatureChangedEventArgs(this, newTargetTemperature));
        }

        private class NullThermostat : NullDevice, IThermostat
        {
            public event EventHandler<ThermostatTargetTemperatureChangedEventArgs> TargetTemperatureChanged = delegate { };

            public Temperature TargetTemperature
            {
                get { return Temperature.Invalid; }

                set { }
            }
        }
    }
}