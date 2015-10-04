namespace Hydra.Cooling.Sensors
{
    using System.Diagnostics.Contracts;

    public class SimulationTemperatureSensor : TemperatureSensor
    {
        private Temperature currentTemperature;

        public SimulationTemperatureSensor()
            : base(new DeviceId(typeof(SimulationTemperatureSensor).Name))
        {
        }

        public override Temperature CurrentTemperature
        {
            get { return this.currentTemperature; }
        }

        public void SetCurrentTemperatur(Temperature newTemperature)
        {
            Contract.Requires(newTemperature != null);

            this.currentTemperature = newTemperature;

            this.OnTemperatureChanged(newTemperature);
        }
    }
}