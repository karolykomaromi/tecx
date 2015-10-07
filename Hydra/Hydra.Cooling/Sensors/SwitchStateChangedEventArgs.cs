namespace Hydra.Cooling.Sensors
{
    public class SwitchStateChangedEventArgs : DeviceStateChangedEventArgs<IEmergencySwitchSensor>
    {
        public SwitchStateChangedEventArgs(IEmergencySwitchSensor device)
            : base(device)
        {
        }

        public SwitchState NewSwitchState { get; set; }
    }
}