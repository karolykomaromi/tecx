namespace Hydra.Cooling.Sensors
{
    using System;

    public interface IEmergencySwitchSensor : ISensor<IEmergencySwitchSensor, SwitchStateChangedEventArgs>
    {
        event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;

        SwitchState CurrentSwitchState { get; }
    }
}