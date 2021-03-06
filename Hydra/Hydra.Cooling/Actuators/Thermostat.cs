namespace Hydra.Cooling.Actuators
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public abstract class Thermostat : Device, IThermostat
    {
        public static new readonly IThermostat Null = new NullThermostat();

        private readonly IObservable<EventPattern<ThermostatTargetTemperatureChangedEventArgs>> observable;

        protected Thermostat(DeviceId id)
            : base(id)
        {
            this.observable = this.ToObservable();
        }

        public event EventHandler<ThermostatTargetTemperatureChangedEventArgs> TargetTemperatureChanged = delegate { };

        public abstract Temperature TargetTemperature { get; set; }

        public virtual IDisposable Subscribe(IObserver<EventPattern<ThermostatTargetTemperatureChangedEventArgs>> observer)
        {
            return this.observable.Subscribe(observer);
        }

        protected virtual IObservable<EventPattern<ThermostatTargetTemperatureChangedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<ThermostatTargetTemperatureChangedEventArgs>(
                handler => this.TargetTemperatureChanged += handler,
                handler => this.TargetTemperatureChanged -= handler);
        }

        protected virtual void OnTargetTemperatureChanged(ThermostatTargetTemperatureChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.TargetTemperatureChanged(this, args);
        }

        protected virtual void OnTargetTemperatureChanged(Temperature oldTargetTemperature, Temperature newTargetTemperature)
        {
            Contract.Requires(newTargetTemperature != null);
            Contract.Requires(oldTargetTemperature != null);

            this.TargetTemperatureChanged(this, new ThermostatTargetTemperatureChangedEventArgs(this, oldTargetTemperature, newTargetTemperature));
        }

        private class NullThermostat : NullDevice, IThermostat
        {
            public event EventHandler<ThermostatTargetTemperatureChangedEventArgs> TargetTemperatureChanged = delegate { };

            public Temperature TargetTemperature
            {
                get { return Temperature.Invalid; }

                set { }
            }

            public IDisposable Subscribe(IObserver<EventPattern<ThermostatTargetTemperatureChangedEventArgs>> observer)
            {
                return Disposable.Empty;
            }
        }
    }
}