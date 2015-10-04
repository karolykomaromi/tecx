namespace Hydra.Cooling.Sensors
{
    using System;

    public interface IDoorSensor : IDevice
    {
        DoorState CurrentDoorState { get; }

        event EventHandler<DoorStateChangedEventArgs> DoorStateChanged;
    }
}