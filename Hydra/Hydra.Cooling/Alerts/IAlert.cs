namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using Hydra.Cooling.Sensors;
    using Hydra.Infrastructure;

    [ContractClass(typeof(AlertContract<>))]
    public interface IAlert<TEventArgs> : IObservable<EventPattern<TEventArgs>>
        where TEventArgs : SensorStateChangedEventArgs
    {
    }

    [ContractClassFor(typeof(IAlert<>))]
    internal abstract class AlertContract<TEventArgs> : IAlert<TEventArgs>
        where TEventArgs : SensorStateChangedEventArgs
    {
        public IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            Contract.Requires(observer != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return Disposable.Empty;
        }
    }
}