﻿namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(TemperatureSensorContract))]
    public interface ITemperatureSensor : ISensor<ITemperatureSensor, TemperatureChangedEventArgs>
    {
        Temperature CurrentTemperature { get; }

        event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;
    }

    [ContractClassFor(typeof(ITemperatureSensor))]
    internal abstract class TemperatureSensorContract : SensorContract<ITemperatureSensor, TemperatureChangedEventArgs>, ITemperatureSensor
    {
        public Temperature CurrentTemperature
        {
            get
            {
                Contract.Ensures(Contract.Result<Temperature>() != null);
                return Temperature.Invalid;
            }
        }

        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };
    }
}
