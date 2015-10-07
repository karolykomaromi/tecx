namespace Hydra.Cooling
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public abstract class Sensor<TSensor, TEventArgs> : Device, ISensor<TSensor, TEventArgs> 
        where TEventArgs : DeviceStateChangedEventArgs<TSensor> 
        where TSensor : ISensor<TSensor, TEventArgs>
    {
        public static readonly new ISensor<TSensor, TEventArgs> Null = new NullSensor<TSensor, TEventArgs>();

        private readonly IObservable<EventPattern<TEventArgs>> observable;

        protected Sensor(DeviceId id) 
            : base(id)
        {
            this.observable = this.ToObservable();
        }

        public virtual IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            return this.observable.Subscribe(observer);
        }

        protected abstract IObservable<EventPattern<TEventArgs>> ToObservable();

        protected class NullSensor<TDevice, TArgs> : Sensor<TDevice, TArgs>
            where TDevice : ISensor<TDevice, TArgs>
            where TArgs : DeviceStateChangedEventArgs<TDevice>
        {
            public NullSensor()
                : base(DeviceId.Empty)
            {
            }

            protected sealed override IObservable<EventPattern<TArgs>> ToObservable()
            {
                return Observable.Empty<EventPattern<TArgs>>();
            }
        }
    }
}