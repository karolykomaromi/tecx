namespace Hydra.Cooling
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using Hydra.Cooling.Sensors;
    using Hydra.Infrastructure;

    [ContractClass(typeof(SensorContract<,>))]
    public interface ISensor<TSensor, TEventArgs> : IDevice, IObservable<EventPattern<TEventArgs>>
        where TEventArgs : DeviceStateChangedEventArgs<TSensor>
        where TSensor : ISensor<TSensor, TEventArgs>
    {
    }

    [ContractClassFor(typeof(ISensor<,>))]
    internal abstract class SensorContract<TSensor, TEventArgs> : DeviceContract, ISensor<TSensor, TEventArgs> 
        where TSensor : ISensor<TSensor, TEventArgs> 
        where TEventArgs : DeviceStateChangedEventArgs<TSensor>
    {
        public IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            Contract.Requires(observer != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return Disposable.Empty;
        }
    }
}