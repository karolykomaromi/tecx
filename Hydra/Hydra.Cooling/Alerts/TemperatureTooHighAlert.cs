namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public class TemperatureTooHighAlert : SensorStateChangedAlert<ITemperatureSensor, TemperatureTooHighEventArgs>
    {
        private readonly Temperature maxAllowedTemperature;

        public TemperatureTooHighAlert(ITemperatureSensor device, Temperature maxAllowedTemperature)
            : base(device)
        {
            Contract.Requires(maxAllowedTemperature != null);

            this.maxAllowedTemperature = maxAllowedTemperature;
            this.Device.TemperatureChanged += this.OnTemperatureChanged;
        }

        public event EventHandler<TemperatureTooHighEventArgs> TemperatureTooHigh = delegate { };

        public Temperature MaxAllowedTemperature
        {
            get { return this.maxAllowedTemperature; }
        }

        protected virtual void OnTemperatureTooHigh(TemperatureTooHighEventArgs args)
        {
            Contract.Requires(args != null);

            this.TemperatureTooHigh(this, args);
        }

        protected virtual void OnTemperatureTooHigh(Temperature actualTemperature)
        {
            Contract.Requires(actualTemperature != null);

            this.TemperatureTooHigh(this, new TemperatureTooHighEventArgs(this.Device, this.MaxAllowedTemperature, actualTemperature));
        }

        protected override IObservable<EventPattern<TemperatureTooHighEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<TemperatureTooHighEventArgs>(
                handler => this.TemperatureTooHigh += handler,
                handler => this.TemperatureTooHigh -= handler);
        }

        private void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
        {
            if (e.NewTemperature.ToCelsius() > this.MaxAllowedTemperature.ToCelsius())
            {
                this.TemperatureTooHigh(this, new TemperatureTooHighEventArgs(this.Device, this.MaxAllowedTemperature, e.NewTemperature));
            }
        }
    }
}