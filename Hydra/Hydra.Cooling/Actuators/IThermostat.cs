namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ThermostatContract))]
    public interface IThermostat : IDevice
    {
        event EventHandler<ThermostatTargetTemperatureChangedEventArgs> TargetTemperatureChanged;
        
        Temperature TargetTemperature { get; set; }
    }

    [ContractClassFor(typeof(IThermostat))]
    internal abstract class ThermostatContract : DeviceContract, IThermostat
    {
        public event EventHandler<ThermostatTargetTemperatureChangedEventArgs> TargetTemperatureChanged = delegate { };

        public Temperature TargetTemperature
        {
            get
            {
                Contract.Ensures(Contract.Result<Temperature>() != null); 
                return Temperature.Invalid;
            }

            set
            {
                Contract.Requires(value != null);
            }
        }
    }
}