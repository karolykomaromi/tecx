namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Xunit;

    public class EmergencySwitchPressedAlertTests
    {
        [Fact]
        public void Should_Raise_Event_When_Switch_Is_Pressed()
        {
            var sensor = new SimulationEmergencySwitchSensor();

            EmergencySwitchPressedAlert alert = new EmergencySwitchPressedAlert(sensor);

            bool eventRaised = false;

            alert.EmergencySwitchPressed += (s, e) => eventRaised = true;

            sensor.SetCurrentSwitchState(SwitchState.On);

            Assert.True(eventRaised);
        }
    }
}