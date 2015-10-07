namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;

    public abstract class EmergencySwitchSensor : Sensor<IEmergencySwitchSensor, SwitchStateChangedEventArgs>, IEmergencySwitchSensor
    {
        public static new readonly IEmergencySwitchSensor Null = new NullEmergencySwitchSensor();

        protected EmergencySwitchSensor(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged = delegate { };

        public abstract SwitchState CurrentSwitchState { get; }
        
        protected sealed override IObservable<EventPattern<SwitchStateChangedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<SwitchStateChangedEventArgs>(
                handler => this.SwitchStateChanged += handler,
                handler => this.SwitchStateChanged -= handler);
        }

        protected virtual void OnSwitchStateChanged(SwitchStateChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.SwitchStateChanged(this, args);
        }

        protected virtual void OnSwitchStateChanged(SwitchState newSwitchState)
        {
            this.SwitchStateChanged(this, new SwitchStateChangedEventArgs(this) { NewSwitchState = newSwitchState });
        }

        private class NullEmergencySwitchSensor : NullSensor<IEmergencySwitchSensor, SwitchStateChangedEventArgs>, IEmergencySwitchSensor
        {
            public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged = delegate { };

            public SwitchState CurrentSwitchState
            {
                get { return SwitchState.Off; }
            }
        }
    }
}