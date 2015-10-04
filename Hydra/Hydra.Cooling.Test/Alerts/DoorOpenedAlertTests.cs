namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Xunit;

    public class DoorOpenedAlertTests
    {
        [Fact]
        public void Should_Raise_Event_When_Door_Opens()
        {
            var sensor = new SimulationDoorSensor();

            DoorOpenedAlert alert = new DoorOpenedAlert(sensor);

            bool eventRaised = false;

            alert.DoorOpened += (s, e) => eventRaised = true;

            sensor.SetCurrentDoorState(DoorState.Open);

            Assert.True(eventRaised);
        }
    }
}
