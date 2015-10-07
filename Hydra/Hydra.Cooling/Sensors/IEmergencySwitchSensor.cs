namespace Hydra.Cooling.Sensors
{
    using System;

    public interface IEmergencySwitchSensor : ISensor<IEmergencySwitchSensor, SwitchStateChangedEventArgs>
    {
        SwitchState CurrentSwitchState { get; }

        event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;
    }
}