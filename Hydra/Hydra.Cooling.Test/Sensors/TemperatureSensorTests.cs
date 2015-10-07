namespace Hydra.Cooling.Test.Sensors
{
    using System;
    using System.Reactive;
    using Hydra.Cooling.Sensors;
    using Moq;
    using Xunit;

    public class TemperatureSensorTests
    {
        [Fact]
        public void Should_Raise_TemperatureChanged_Event()
        {
            var sensor = new SimulationTemperatureSensor();

            bool eventRaised = false;

            sensor.TemperatureChanged += (s, e) => eventRaised = true;

            sensor.SetCurrentTemperatur(25.DegreesCelsius());

            Assert.True(eventRaised);
        }

        [Fact]
        public void Should_Call_Observer_On_TemperatureChanged()
        {
            var sensor = new SimulationTemperatureSensor();

            var observer = new Mock<IObserver<EventPattern<TemperatureChangedEventArgs>>>();

            using (IDisposable subscription = sensor.Subscribe(observer.Object))
            {
                sensor.SetCurrentTemperatur(25.DegreesCelsius());
            }

            sensor.SetCurrentTemperatur(30.DegreesCelsius());

            observer.Verify(o => o.OnNext(It.IsAny<EventPattern<TemperatureChangedEventArgs>>()), Times.Once);
            observer.Verify(o => o.OnNext(It.Is<EventPattern<TemperatureChangedEventArgs>>(e => e.EventArgs.NewTemperature.ToCelsius() == 25.DegreesCelsius())), Times.Once);
        }
    }
}
