namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public class EmergencySwitchReleasedAlert : SensorStateChangedAlert<IEmergencySwitchSensor, EmergencySwitchReleasedEventArgs>
    {
        public EmergencySwitchReleasedAlert(IEmergencySwitchSensor device)
            : base(device)
        {
            this.Device.SwitchStateChanged += this.OnSwitchStateChanged;
        }

        public event EventHandler<EmergencySwitchReleasedEventArgs> EmergencySwitchReleased = delegate { };

        protected virtual void OnEmergencySwitchReleased(EmergencySwitchReleasedEventArgs args)
        {
            Contract.Requires(args != null);

            this.EmergencySwitchReleased(this, args);
        }

        protected override IObservable<EventPattern<EmergencySwitchReleasedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<EmergencySwitchReleasedEventArgs>(
                handler => this.EmergencySwitchReleased += handler,
                handler => this.EmergencySwitchReleased -= handler);
        }

        protected virtual void OnEmergencySwitchReleased()
        {
            this.EmergencySwitchReleased(this, new EmergencySwitchReleasedEventArgs(this.Device));
        }

        private void OnSwitchStateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            if (e.NewSwitchState == SwitchState.On)
            {
                this.EmergencySwitchReleased(this, new EmergencySwitchReleasedEventArgs(this.Device));
            }
        }
    }
}