using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Event;
using TecX.Event.Unity;
using TecX.Common.Test.TestObjects;
using TecX.TestTools;

namespace TecX.Common.Test
{
    [TestClass]
    public class EventAggregatorContainerExtensionFixture
    {
        [TestMethod]
        public void CanAutoSubscribeUsingUnity()
        {
            IUnityContainer container = new UnityContainer();
            
            container.AddNewExtension<EventAggregatorExtension>();

            IEventAggregator eventAggregator = container.Resolve<IEventAggregator>();

            Assert.IsNotNull(eventAggregator);

            SimpleSubscriber subscriber = container.Resolve<SimpleSubscriber>();

            eventAggregator.Publish(new SimpleMessage());

            DispatcherUtil.DoEvents();

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void CanPublishUsingUnityInfrastructure()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<EventAggregatorExtension>();

            SimpleSubscriber subscriber = container.Resolve<SimpleSubscriber>();

            SimplePublisher publisher = container.Resolve<SimplePublisher>();

            publisher.Publish();

            DispatcherUtil.DoEvents();

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }
    }
}
