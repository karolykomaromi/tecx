namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Disposables;

    [ContractClass(typeof(ThermostatContract))]
    public interface IThermostat : IDevice, IObservable<EventPattern<ThermostatTargetTemperatureChangedEventArgs>>
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

        public IDisposable Subscribe(IObserver<EventPattern<ThermostatTargetTemperatureChangedEventArgs>> observer)
        {
            Contract.Requires(observer != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return Disposable.Empty;
        }
    }
}