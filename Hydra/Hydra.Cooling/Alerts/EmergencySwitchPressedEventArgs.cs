namespace Hydra.Cooling.Alerts
{
    using Hydra.Cooling.Sensors;

    public class EmergencySwitchPressedEventArgs : SensorStateChangedEventArgs
    {
        public EmergencySwitchPressedEventArgs(IEmergencySwitchSensor device)
            : base(device)
        {
        }
    }
}