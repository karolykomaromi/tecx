namespace Hydra.Cooling.Test.Alerts
{
    using System;
    using System.Linq;
    using System.Reactive;
    using Hydra.Cooling.Alerts;
    using Hydra.Cooling.Sensors;
    using Moq;
    using Xunit;

    public class CompositeAlertTests
    {
        [Fact]
        public void Should_Expand_Composite_When_Adding_To_Other_Composite()
        {
            TemperatureTooHighAlert alert1 = new TemperatureTooHighAlert(TemperatureSensor.Null, 25.DegreesCelsius());
            TemperatureTooHighAlert alert2 = new TemperatureTooHighAlert(TemperatureSensor.Null, 30.DegreesCelsius());

            CompositeAlert<TemperatureTooHighEventArgs> c1 = new CompositeAlert<TemperatureTooHighEventArgs>(alert1, alert2);

            CompositeAlert<TemperatureTooHighEventArgs> c2 = new CompositeAlert<TemperatureTooHighEventArgs>(c1);

            Assert.Equal(2, c2.Count);
            Assert.Same(alert1, c2.ElementAt(0));
            Assert.Same(alert2, c2.ElementAt(1));
        }

        [Fact]
        public void Should_Create_Composite_Subscription()
        {
            var sensor = new SimulationTemperatureSensor();

            TemperatureTooHighAlert alert1 = new TemperatureTooHighAlert(sensor, 25.DegreesCelsius());
            TemperatureTooHighAlert alert2 = new TemperatureTooHighAlert(sensor, 30.DegreesCelsius());

            CompositeAlert<TemperatureTooHighEventArgs> composite = new CompositeAlert<TemperatureTooHighEventArgs>(alert1, alert2);

            var observer = new Mock<IObserver<EventPattern<TemperatureTooHighEventArgs>>>();

            IDisposable subscription = composite.Subscribe(observer.Object);

            sensor.SetCurrentTemperatur(31.DegreesCelsius());

            subscription.Dispose();

            sensor.SetCurrentTemperatur(34.DegreesCelsius());

            observer.Verify(
                o => o.OnNext(It.Is<EventPattern<TemperatureTooHighEventArgs>>(x => x.EventArgs.MaxAllowTemperature.ToCelsius() == 25.DegreesCelsius())), 
                Times.Once);

            observer.Verify(
                o => o.OnNext(It.Is<EventPattern<TemperatureTooHighEventArgs>>(x => x.EventArgs.MaxAllowTemperature.ToCelsius() == 30.DegreesCelsius())), 
                Times.Once);

            observer.Verify(o => o.OnNext(It.IsAny<EventPattern<TemperatureTooHighEventArgs>>()), Times.Exactly(2));
        }
    }
}
