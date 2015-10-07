namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Linq;

    public abstract class DoorSensor : Sensor<IDoorSensor, DoorStateChangedEventArgs>, IDoorSensor
    {
        public static new readonly IDoorSensor Null = new NullDoorSensor();

        protected DoorSensor(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<DoorStateChangedEventArgs> DoorStateChanged = delegate { };

        public abstract DoorState CurrentDoorState { get; }

        protected sealed override IObservable<EventPattern<DoorStateChangedEventArgs>> ToObservable()
        {
            return Observable.FromEventPattern<DoorStateChangedEventArgs>(
                handler => this.DoorStateChanged += handler,
                handler => this.DoorStateChanged -= handler);
        }

        protected virtual void OnDoorStateChanged(DoorStateChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.DoorStateChanged(this, args);
        }

        protected virtual void OnDoorStateChanged(DoorState newDoorState)
        {
            this.DoorStateChanged(this, new DoorStateChangedEventArgs(this) { NewDoorState = newDoorState });
        }

        private class NullDoorSensor : NullSensor<IDoorSensor, DoorStateChangedEventArgs>, IDoorSensor
        {
            public event EventHandler<DoorStateChangedEventArgs> DoorStateChanged = delegate { };

            public DoorState CurrentDoorState
            {
                get { return DoorState.Closed; }
            }
        }
    }
}