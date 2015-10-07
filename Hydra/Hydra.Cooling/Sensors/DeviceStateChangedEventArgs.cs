namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure;

    public abstract class DeviceStateChangedEventArgs<TDevice> : EventArgs
        where TDevice : IDevice
    {
        private readonly DateTime timestamp;

        private readonly TimeZoneInfo timezone;

        private readonly TDevice device;

        protected DeviceStateChangedEventArgs(TDevice device)
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