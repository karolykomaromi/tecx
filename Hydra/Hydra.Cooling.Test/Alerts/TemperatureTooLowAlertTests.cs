namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Xunit;

    public class TemperatureTooLowAlertTests
    {
        [Fact]
        public void Should_Raise_Event_When_Temperatur_Gets_Too_Low()
        {
            var sensor = new SimulationTemperatureSensor();

            TemperatureTooLowAlert alert = new TemperatureTooLowAlert(sensor, 25.DegreesCelsius());

            bool eventRaised = false;

            alert.TemperatureTooLow += (s, e) => eventRaised = true;

            sensor.SetCurrentTemperatur(20.DegreesCelsius());

            Assert.True(eventRaised);
        }
    }
}