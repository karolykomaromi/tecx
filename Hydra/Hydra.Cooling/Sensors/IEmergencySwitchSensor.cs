namespace Hydra.Cooling.Sensors
{
    using System;

    public interface IEmergencySwitchSensor : IDevice
    {
        SwitchState CurrentSwitchState { get; }

        event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;
    }
}