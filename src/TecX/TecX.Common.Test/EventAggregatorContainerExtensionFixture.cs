using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Event;
using TecX.Common.Event.Unity;
using TecX.Common.Test.TestClasses;
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
            
            container.AddNewExtension<EventAggregatorContainerExtension>();

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
            container.AddNewExtension<EventAggregatorContainerExtension>();

            SimpleSubscriber subscriber = container.Resolve<SimpleSubscriber>();

            SimplePublisher publisher = container.Resolve<SimplePublisher>();

            publisher.Publish();

            DispatcherUtil.DoEvents();

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }
    }
}
