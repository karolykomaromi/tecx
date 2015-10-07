namespace Hydra.Cooling.Sensors
{
    using System;

    public interface IDoorSensor : ISensor<IDoorSensor, DoorStateChangedEventArgs>
    {
        DoorState CurrentDoorState { get; }

        event EventHandler<DoorStateChangedEventArgs> DoorStateChanged;
    }
}