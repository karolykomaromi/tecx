namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class TemperatureSensor : Device, ITemperatureSensor
    {
        public static new readonly ITemperatureSensor Null = new NullTemperatureSensor();

        protected TemperatureSensor(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };

        public abstract Temperature CurrentTemperature { get; }

        protected virtual void OnTemperatureChanged(TemperatureChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.TemperatureChanged(this, args);
        }

        protected virtual void OnTemperatureChanged(Temperature newTemperature)
        {
            Contract.Requires(newTemperature != null);

            this.TemperatureChanged(this, new TemperatureChangedEventArgs(this, newTemperature));
        }

        private class NullTemperatureSensor : NullDevice, ITemperatureSensor
        {
            public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };

            public Temperature CurrentTemperature
            {
                get { return Temperature.Invalid; }
            }
        }
    }
}