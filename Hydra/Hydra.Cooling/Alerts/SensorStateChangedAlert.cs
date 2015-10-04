namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using Hydra.Cooling.Sensors;

    public abstract class SensorStateChangedAlert<TDevice, TEventArgs> : IAlert<TEventArgs>
        where TDevice : IDevice
        where TEventArgs : SensorStateChangedEventArgs
    {
        private readonly TDevice device;

        private readonly IObservable<EventPattern<TEventArgs>> observable;

        protected SensorStateChangedAlert(TDevice device)
        {
            Contract.Requires(device != null);

            this.device = device;
            this.observable = this.ToObservable();
        }

        public TDevice Device
        {
            get { return this.device; }
        }

        public virtual IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            return this.observable.Subscribe(observer);
        }

        protected abstract IObservable<EventPattern<TEventArgs>> ToObservable();
    }
}