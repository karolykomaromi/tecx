namespace Hydra.Cooling.Alerts
{
    using Hydra.Cooling.Sensors;

    public class DoorOpenedEventArgs : SensorStateChangedEventArgs
    {
        public DoorOpenedEventArgs(IDoorSensor device)
            : base(device)
        {
        }
    }
}