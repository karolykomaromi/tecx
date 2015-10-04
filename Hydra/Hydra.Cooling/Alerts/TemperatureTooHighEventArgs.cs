namespace Hydra.Cooling.Alerts
{
    using System.Diagnostics.Contracts;
    using Hydra.Cooling.Sensors;

    public class TemperatureTooHighEventArgs : SensorStateChangedEventArgs
    {
        private readonly Temperature maxAllowTemperature;

        private readonly Temperature actualTemperature;

        public TemperatureTooHighEventArgs(ITemperatureSensor device, Temperature maxAllowTemperature, Temperature actualTemperature)
            : base(device)
        {
            Contract.Requires(maxAllowTemperature != null);
            Contract.Requires(actualTemperature != null);

            this.maxAllowTemperature = maxAllowTemperature.ToCelsius();
            this.actualTemperature = actualTemperature.ToCelsius();
        }

        public Temperature MaxAllowTemperature
        {
            get { return this.maxAllowTemperature; }
        }

        public Temperature ActualTemperature
        {
            get { return this.actualTemperature; }
        }
    }
}
