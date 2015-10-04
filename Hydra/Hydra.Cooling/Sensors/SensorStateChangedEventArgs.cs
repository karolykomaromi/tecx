namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure;

    public abstract class SensorStateChangedEventArgs : EventArgs
    {
        private readonly DateTime timestamp;

        private readonly TimeZoneInfo timezone;

        private readonly IDevice device;

        protected SensorStateChangedEventArgs(IDevice device)
        {
            Contract.Requires(device != null);

            this.device = device;
            this.timestamp = TimeProvider.Now;
            this.timezone = TimeZoneProvider.Local;
        }

        public DateTime Timestamp
        {
            get { return this.timestamp; }
        }

        public TimeZoneInfo Timezone
        {
            get { return this.timezone; }
        }

        public IDevice Device
        {
            get { return this.device; }
        }
    }
}