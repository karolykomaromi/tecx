namespace Hydra.Cooling.Test.Sensors
{
    using System;
    using System.Reactive;
    using Hydra.Cooling.Sensors;
    using Moq;
    using Xunit;

    public class EmergencySwitchSensorTests
    {
        [Fact]
        public void Should_Raise_TemperatureChanged_Event()
        {
            var sensor = new SimulationEmergencySwitchSensor();

            bool eventRaised = false;

            sensor.SwitchStateChanged += (s, e) => eventRaised = true;

            sensor.SetCurrentSwitchState(SwitchState.On);

            Assert.True(eventRaised);
        }

        [Fact]
        public void Should_Call_Observer_On_TemperatureChanged()
        {
            var sensor = new SimulationEmergencySwitchSensor();

            var observer = new Mock<IObserver<EventPattern<SwitchStateChangedEventArgs>>>();

            using (IDisposable subscription = sensor.Subscribe(observer.Object))
            {
                sensor.SetCurrentSwitchState(SwitchState.On);
            }

            sensor.SetCurrentSwitchState(SwitchState.Off);

            observer.Verify(o => o.OnNext(It.IsAny<EventPattern<SwitchStateChangedEventArgs>>()), Times.Once);
            observer.Verify(o => o.OnNext(It.Is<EventPattern<SwitchStateChangedEventArgs>>(e => e.EventArgs.NewSwitchState == SwitchState.On)), Times.Once);
        }
    }
}