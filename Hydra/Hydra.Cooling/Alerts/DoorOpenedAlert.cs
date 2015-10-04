namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public class DoorOpenedAlert : SensorStateChangedAlert<IDoorSensor, DoorOpenedEventArgs>
    {
        public DoorOpenedAlert(IDoorSensor device)
            : base(device)
        {
            this.Device.DoorStateChanged += this.OnDoorStateChanged;
        }

        public event EventHandler<DoorOpenedEventArgs> DoorOpened = delegate { };

        protected virtual void OnDoorOpened(DoorOpenedEventArgs args)
        {
            Contract.Requires(args != null);

            this.DoorOpened(this, args);
        }

        protected virtual void OnDoorOpened()
        {
            this.DoorOpened(this, new DoorOpenedEventArgs(this.Device));
        }

        protected override IObservable<EventPattern<DoorOpenedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<DoorOpenedEventArgs>(
                handler => this.DoorOpened += handler,
                handler => this.DoorOpened -= handler);
        }

        private void OnDoorStateChanged(object sender, DoorStateChangedEventArgs e)
        {
            if (e.NewDoorState == DoorState.Open)
            {
                this.DoorOpened(this, new DoorOpenedEventArgs(this.Device));
            }
        }
    }
}