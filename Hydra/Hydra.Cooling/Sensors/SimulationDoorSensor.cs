namespace Hydra.Cooling.Sensors
{
    public class SimulationDoorSensor : DoorSensor
    {
        private DoorState currentDoorState;

        public SimulationDoorSensor()
            : base(new DeviceId(typeof(SimulationDoorSensor).Name))
        {
        }

        public override DoorState CurrentDoorState
        {
            get { return this.currentDoorState; }
        }

        public void SetCurrentDoorState(DoorState newDoorState)
        {
            this.currentDoorState = newDoorState;

            this.OnDoorStateChanged(newDoorState);
        }
    }
}