namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public class EmergencySwitchPressedAlert : SensorStateChangedAlert<IEmergencySwitchSensor, EmergencySwitchPressedEventArgs>
    {
        public EmergencySwitchPressedAlert(IEmergencySwitchSensor device)
            : base(device)
        {
            this.Device.SwitchStateChanged += this.OnSwitchStateChanged;
        }

        public event EventHandler<EmergencySwitchPressedEventArgs> EmergencySwitchPressed = delegate { };

        protected virtual void OnEmergencySwitchPressed(EmergencySwitchPressedEventArgs args)
        {
            Contract.Requires(args != null);

            this.EmergencySwitchPressed(this, args);
        }

        protected virtual void OnEmergencySwitchPressed()
        {
            this.EmergencySwitchPressed(this, new EmergencySwitchPressedEventArgs(this.Device));
        }

        protected override IObservable<EventPattern<EmergencySwitchPressedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<EmergencySwitchPressedEventArgs>(
                handler => this.EmergencySwitchPressed += handler,
                handler => this.EmergencySwitchPressed -= handler);
        }

        private void OnSwitchStateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            if (e.NewSwitchState == SwitchState.On)
            {
                this.EmergencySwitchPressed(this, new EmergencySwitchPressedEventArgs(this.Device));
            }
        }
    }
}