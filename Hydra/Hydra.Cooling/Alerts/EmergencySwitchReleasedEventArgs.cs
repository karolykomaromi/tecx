namespace Hydra.Cooling.Alerts
{
    using Hydra.Cooling.Sensors;

    public class EmergencySwitchReleasedEventArgs : SensorStateChangedEventArgs
    {
        public EmergencySwitchReleasedEventArgs(IEmergencySwitchSensor device)
            : base(device)
        {
        }
    }
}