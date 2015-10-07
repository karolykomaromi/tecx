namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(TemperatureSensorContract))]
    public interface ITemperatureSensor : ISensor<ITemperatureSensor, TemperatureChangedEventArgs>
    {
        event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;

        Temperature CurrentTemperature { get; }
    }

    [ContractClassFor(typeof(ITemperatureSensor))]
    internal abstract class TemperatureSensorContract : SensorContract<ITemperatureSensor, TemperatureChangedEventArgs>, ITemperatureSensor
    {
        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };

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
