namespace Hydra.Cooling.Sensors
{
    using System.Diagnostics.Contracts;

    public class SimulationTemperatureSensor : TemperatureSensor
    {
        private Temperature currentTemperature;

        public SimulationTemperatureSensor()
            : base(new DeviceId(byte.MaxValue))
        {
        }

        public override Temperature CurrentTemperature
        {
            get { return this.currentTemperature; }
        }

        public void SetCurrentTemperatur(Temperature newTemperature)
        {
            Contract.Requires(newTemperature != null);

            Temperature oldTemperature = this.currentTemperature;

            this.currentTemperature = newTemperature;

            this.OnTemperatureChanged(oldTemperature, newTemperature);
        }
    }
}