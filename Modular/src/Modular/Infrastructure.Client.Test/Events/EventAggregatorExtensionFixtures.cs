namespace Infrastructure.Client.Test.Events
{
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.Events;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class EventAggregatorExtensionFixtures
    {
        [TestMethod]
        public void Should_Auto_Subscribe_ViewModel()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<EventAggregatorExtension>();

            var ea = new Mock<IEventAggregator>();

            container.RegisterInstance<IEventAggregator>(ea.Object);

            Subscriber subscriber = container.Resolve<Subscriber>();

            ea.Verify(e => e.Subscribe(subscriber), Times.Once);
        }
    }
}
