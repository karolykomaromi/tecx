namespace Hydra.Cooling.Sensors
{
    public class SwitchStateChangedEventArgs : SensorStateChangedEventArgs
    {
        public SwitchStateChangedEventArgs(IDevice device)
            : base(device)
        {
        }

        public SwitchState NewSwitchState { get; set; }
    }
}