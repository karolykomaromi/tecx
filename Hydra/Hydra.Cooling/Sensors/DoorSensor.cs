namespace Hydra.Cooling.Sensors
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class DoorSensor : Device, IDoorSensor
    {
        public static new readonly IDoorSensor Null = new NullDoorSensor();

        protected DoorSensor(DeviceId id)
            : base(id)
        {
        }

        public event EventHandler<DoorStateChangedEventArgs> DoorStateChanged = delegate { };

        public abstract DoorState CurrentDoorState { get; }

        protected virtual void OnDoorStateChanged(DoorStateChangedEventArgs args)
        {
            Contract.Requires(args != null);

            this.DoorStateChanged(this, args);
        }

        protected virtual void OnDoorStateChanged(DoorState newDoorState)
        {
            this.DoorStateChanged(this, new DoorStateChangedEventArgs(this) { NewDoorState = newDoorState });
        }

        private class NullDoorSensor : NullDevice, IDoorSensor
        {
            public event EventHandler<DoorStateChangedEventArgs> DoorStateChanged = delegate { };

            public DoorState CurrentDoorState
            {
                get { return DoorState.Closed; }
            }
        }
    }
}