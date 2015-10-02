namespace Hydra.Cooling
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(TemperatureSensorContract))]
    public interface ITemperatureSensor : IDevice
    {
        Temperature CurrentTemperature { get; }
    }

    [ContractClassFor(typeof(ITemperatureSensor))]
    internal abstract class TemperatureSensorContract : DeviceContract, ITemperatureSensor
    {
        public Temperature CurrentTemperature
        {
            get
            {
                Contract.Ensures(Contract.Result<Temperature>() != null);
                return Temperature.Invalid;
            }
        }
    }
}
