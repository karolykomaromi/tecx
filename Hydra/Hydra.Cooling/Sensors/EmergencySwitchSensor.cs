namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class EmergencySwitchSensor : Device, IEmergencySwitchSensor
    {
        public static new readonly IEmergencySwitchSensor Null = new NullEmergencySwitchSensor();

        protected EmergencySwitchSensor(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged = delegate { };

        public abstract SwitchState CurrentSwitchState { get; }

        protected virtual void OnSwitchStateChanged(SwitchStateChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.SwitchStateChanged(this, args);
        }

        protected virtual void OnSwitchStateChanged(SwitchState newSwitchState)
        {
            this.SwitchStateChanged(this, new SwitchStateChangedEventArgs(this) { NewSwitchState = newSwitchState });
        }

        private class NullEmergencySwitchSensor : NullDevice, IEmergencySwitchSensor
        {
            public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged = delegate { };

            public SwitchState CurrentSwitchState
            {
                get { return SwitchState.Off; }
            }
        }
    }
}