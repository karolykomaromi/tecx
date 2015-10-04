namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public class TemperatureTooLowAlert : SensorStateChangedAlert<ITemperatureSensor, TemperatureTooLowEventArgs>
    {
        private readonly Temperature minAllowedTemperature;

        public TemperatureTooLowAlert(ITemperatureSensor device, Temperature minAllowedTemperature)
            : base(device)
        {
            Contract.Requires(minAllowedTemperature != null);

            this.minAllowedTemperature = minAllowedTemperature;
            this.Device.TemperatureChanged += this.OnTemperatureChanged;
        }

        public event EventHandler<TemperatureTooLowEventArgs> TemperatureTooLow = delegate { };

        public Temperature MinAllowedTemperature
        {
            get { return this.minAllowedTemperature; }
        }

        protected virtual void OnTemperatureTooLow(TemperatureTooLowEventArgs args)
        {
            Contract.Requires(args != null);

            this.TemperatureTooLow(this, args);
        }

        protected virtual void OnTemperatureTooLow(Temperature actualTemperature)
        {
            Contract.Requires(actualTemperature != null);

            this.TemperatureTooLow(this, new TemperatureTooLowEventArgs(this.Device, this.MinAllowedTemperature, actualTemperature));
        }

        protected sealed override IObservable<EventPattern<TemperatureTooLowEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<TemperatureTooLowEventArgs>(
                handler => this.TemperatureTooLow += handler,
                handler => this.TemperatureTooLow -= handler);
        }

        private void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
        {
            if (this.MinAllowedTemperature.ToCelsius() > e.NewTemperature.ToCelsius())
            {
                this.TemperatureTooLow(this, new TemperatureTooLowEventArgs(this.Device, this.MinAllowedTemperature, e.NewTemperature));
            }
        }
    }
}