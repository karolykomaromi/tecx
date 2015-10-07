namespace Hydra.Cooling.Test.Sensors
{
    using System;
    using Hydra.Cooling.Sensors;
    using Moq;
    using Xunit;

    public class CompositeObserverTests
    {
        [Fact]
        public void Should_Forward_OnNext()
        {
            var observer1 = new Mock<IObserver<Celsius>>();

            var observer2 = new Mock<IObserver<Celsius>>();

            var composite = new CompositeObserver<Celsius>(observer1.Object, observer2.Object);

            composite.OnNext(25.DegreesCelsius());

            observer1.Verify(o => o.OnNext(It.Is<Celsius>(c => c == 25.DegreesCelsius())), Times.Once);
            observer2.Verify(o => o.OnNext(It.Is<Celsius>(c => c == 25.DegreesCelsius())), Times.Once);
        }

        [Fact]
        public void Should_Forward_OnError()
        {
            var observer1 = new Mock<IObserver<Celsius>>();

            var observer2 = new Mock<IObserver<Celsius>>();

            var composite = new CompositeObserver<Celsius>(observer1.Object, observer2.Object);

            Exception ex = new Exception();

            composite.OnError(ex);

            observer1.Verify(o => o.OnError(ex), Times.Once);
            observer2.Verify(o => o.OnError(ex), Times.Once);
        }

        [Fact]
        public void Should_Forward_OnCompleted()
        {
            var observer1 = new Mock<IObserver<Celsius>>();

            var observer2 = new Mock<IObserver<Celsius>>();

            var composite = new CompositeObserver<Celsius>(observer1.Object, observer2.Object);

            composite.OnCompleted();

            observer1.Verify(o => o.OnCompleted(), Times.Once);
            observer2.Verify(o => o.OnCompleted(), Times.Once);
        }
    }
}
