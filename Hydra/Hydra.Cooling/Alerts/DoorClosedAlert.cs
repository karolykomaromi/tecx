namespace Hydra.Cooling.Alerts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;
    using Hydra.Cooling.Sensors;

    public class DoorClosedAlert : SensorStateChangedAlert<IDoorSensor, DoorClosedEventArgs>
    {
        public DoorClosedAlert(IDoorSensor device)
            : base(device)
        {
            this.Device.DoorStateChanged += this.OnDoorStateChanged;
        }

        public event EventHandler<DoorClosedEventArgs> DoorClosed = delegate { };

        protected virtual void OnDoorClosed(DoorClosedEventArgs args)
        {
            Contract.Requires(args != null);

            this.DoorClosed(this, args);
        }

        protected virtual void OnDoorClosed()
        {
            this.DoorClosed(this, new DoorClosedEventArgs(this.Device));
        }

        protected sealed override IObservable<EventPattern<DoorClosedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<DoorClosedEventArgs>(
                handler => this.DoorClosed += handler,
                handler => this.DoorClosed -= handler);
        }

        private void OnDoorStateChanged(object sender, DoorStateChangedEventArgs e)
        {
            if (e.NewDoorState == DoorState.Closed)
            {
                this.DoorClosed(this, new DoorClosedEventArgs(this.Device));
            }
        }
    }
}