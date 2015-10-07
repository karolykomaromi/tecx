namespace Hydra.Cooling.Test.Sensors
{
    using System;
    using System.Reactive;
    using Hydra.Cooling.Sensors;
    using Moq;
    using Xunit;

    public class DoorSensorTests
    {
        [Fact]
        public void Should_Raise_TemperatureChanged_Event()
        {
            var sensor = new SimulationDoorSensor();

            bool eventRaised = false;

            sensor.DoorStateChanged += (s, e) => eventRaised = true;

            sensor.SetCurrentDoorState(DoorState.Open);

            Assert.True(eventRaised);
        }

        [Fact]
        public void Should_Call_Observer_On_TemperatureChanged()
        {
            var sensor = new SimulationDoorSensor();

            var observer = new Mock<IObserver<EventPattern<DoorStateChangedEventArgs>>>();

            using (IDisposable subscription = sensor.Subscribe(observer.Object))
            {
                sensor.SetCurrentDoorState(DoorState.Open);
            }

            sensor.SetCurrentDoorState(DoorState.Closed);

            observer.Verify(o => o.OnNext(It.IsAny<EventPattern<DoorStateChangedEventArgs>>()), Times.Once);
            observer.Verify(o => o.OnNext(It.Is<EventPattern<DoorStateChangedEventArgs>>(e => e.EventArgs.NewDoorState == DoorState.Open)), Times.Once);
        }
    }
}