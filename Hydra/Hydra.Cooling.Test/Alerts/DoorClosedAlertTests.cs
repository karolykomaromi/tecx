namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Xunit;

    public class DoorClosedAlertTests
    {
        [Fact]
        public void Should_Raise_Event_When_Door_Closes()
        {
            var sensor = new SimulationDoorSensor();

            DoorClosedAlert alert = new DoorClosedAlert(sensor);

            bool eventRaised = false;

            alert.DoorClosed += (s, e) => eventRaised = true;

            sensor.SetCurrentDoorState(DoorState.Closed);

            Assert.True(eventRaised);
        }
    }
}