namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;

    public abstract class TemperatureSensor : Sensor<ITemperatureSensor, TemperatureChangedEventArgs>, ITemperatureSensor
    {
        public static new readonly ITemperatureSensor Null = new NullTemperatureSensor();

        protected TemperatureSensor(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };

        public abstract Temperature CurrentTemperature { get; }

        protected sealed override IObservable<EventPattern<TemperatureChangedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<TemperatureChangedEventArgs>(
                handler => this.TemperatureChanged += handler,
                handler => this.TemperatureChanged -= handler);
        }

        protected virtual void OnTemperatureChanged(TemperatureChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.TemperatureChanged(this, args);
        }

        protected virtual void OnTemperatureChanged(Temperature oldTemperature, Temperature newTemperature)
        {
            Contract.Requires(oldTemperature != null);
            Contract.Requires(newTemperature != null);

            this.TemperatureChanged(this, new TemperatureChangedEventArgs(this, oldTemperature, newTemperature));
        }

        private class NullTemperatureSensor : NullSensor<ITemperatureSensor, TemperatureChangedEventArgs>, ITemperatureSensor
        {
            public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };

            public Temperature CurrentTemperature
            {
                get { return Temperature.Invalid; }
            }
        }
    }
}