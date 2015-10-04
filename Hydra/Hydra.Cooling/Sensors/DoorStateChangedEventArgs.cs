namespace Hydra.Cooling.Sensors
{
    public class DoorStateChangedEventArgs : SensorStateChangedEventArgs
    {
        public DoorStateChangedEventArgs(IDevice device)
            : base(device)
        {
        }

        public DoorState NewDoorState { get; set; }
    }
}