namespace Hydra.Cooling.Alerts
{
    using Hydra.Cooling.Sensors;

    public class DoorClosedEventArgs : SensorStateChangedEventArgs
    {
        public DoorClosedEventArgs(IDoorSensor device)
            : base(device)
        {
        }
    }
}