namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Xunit;

    public class EmergencySwitchReleasedAlertTests
    {
        [Fact]
        public void Should_Raise_Event_When_Switch_Is_Pressed()
        {
            var sensor = new SimulationEmergencySwitchSensor();

            EmergencySwitchReleasedAlert alert = new EmergencySwitchReleasedAlert(sensor);

            bool eventRaised = false;

            alert.EmergencySwitchReleased += (s, e) => eventRaised = true;

            sensor.SetCurrentSwitchState(SwitchState.Off);

            Assert.True(eventRaised);
        }
    }
}