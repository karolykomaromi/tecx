namespace Hydra.Cooling.Sensors
{
    public class SimulationEmergencySwitchSensor : EmergencySwitchSensor
    {
        private SwitchState currentSwitchState;

        public SimulationEmergencySwitchSensor()
            : base(new DeviceId(typeof(SimulationEmergencySwitchSensor).Name))
        {
        }

        public override SwitchState CurrentSwitchState
        {
            get { return this.currentSwitchState; }
        }

        public void SetCurrentSwitchState(SwitchState newSwitchState)
        {
            this.currentSwitchState = newSwitchState;

            this.OnSwitchStateChanged(newSwitchState);
        }
    }
}