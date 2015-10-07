namespace Hydra.Cooling.Sensors
{
    using System;

    public interface IDoorSensor : ISensor<IDoorSensor, DoorStateChangedEventArgs>
    {
        event EventHandler<DoorStateChangedEventArgs> DoorStateChanged;

        DoorState CurrentDoorState { get; }
    }
}