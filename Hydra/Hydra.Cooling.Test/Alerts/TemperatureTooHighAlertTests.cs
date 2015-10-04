namespace Hydra.Cooling.Test.Alerts
{
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Xunit;

    public class TemperatureTooHighAlertTests
    {
        [Fact]
        public void Should_Raise_Event_When_Temperatur_Gets_Too_High()
        {
            var sensor = new SimulationTemperatureSensor();

            TemperatureTooHighAlert alert = new TemperatureTooHighAlert(sensor, 25.DegreesCelsius());

            bool eventRaised = false;

            alert.TemperatureTooHigh += (s, e) => eventRaised = true;

            sensor.SetCurrentTemperatur(30.DegreesCelsius());

            Assert.True(eventRaised);
        }
    }
}