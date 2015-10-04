namespace Hydra.Cooling.Alerts
{
    using System.Diagnostics.Contracts;
    using Hydra.Cooling.Sensors;

    public class TemperatureTooLowEventArgs : SensorStateChangedEventArgs
    {
        private readonly Temperature minAllowedTemperature;

        private readonly Temperature actualTemperature;

        public TemperatureTooLowEventArgs(ITemperatureSensor device, Temperature minAllowedTemperature, Temperature actualTemperature)
            : base(device)
        {
            Contract.Requires(minAllowedTemperature != null);
            Contract.Requires(actualTemperature != null);

            this.minAllowedTemperature = minAllowedTemperature;
            this.actualTemperature = actualTemperature;
        }

        public Temperature MinAllowedTemperature
        {
            get { return this.minAllowedTemperature; }
        }

        public Temperature ActualTemperature
        {
            get { return this.actualTemperature; }
        }
    }
}