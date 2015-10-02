namespace Hydra.Cooling
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ThermostatContract))]
    public interface IThermostat : IDevice
    {
        void SetTargetTemperature(Temperature temperature);
    }

    [ContractClassFor(typeof(IThermostat))]
    internal abstract class ThermostatContract : DeviceContract, IThermostat
    {
        public void SetTargetTemperature(Temperature temperature)
        {
            Contract.Requires(temperature != null);
        }
    }
}