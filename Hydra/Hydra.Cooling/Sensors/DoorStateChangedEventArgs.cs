namespace Hydra.Cooling.Sensors
{
    public class DoorStateChangedEventArgs : DeviceStateChangedEventArgs<IDoorSensor>
    {
        public DoorStateChangedEventArgs(IDoorSensor device)
            : base(device)
        {
        }

        public DoorState NewDoorState { get; set; }
    }
}