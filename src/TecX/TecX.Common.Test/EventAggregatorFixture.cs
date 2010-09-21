using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Event;
using TecX.Common.Test.TestClasses;

namespace TecX.Common.Test
{
    [TestClass]
    public class EventAggregatorFixture
    {
        [TestMethod]
        public void CanPublishMessageOnSameThread()
        {
            SynchronizationContext context = new SynchronizationContext();

            IEventAggregator eventAggregator = new EventAggregator(context);

            SimpleSubscriber subscriber = new SimpleSubscriber();

            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void CanPublishMessageOnDifferentThread()
        {
            SynchronizationContext context = new SynchronizationContext();

            IEventAggregator eventAggregator = new EventAggregator(context);

            SimpleSubscriber subscriber = new SimpleSubscriber();

            eventAggregator.Subscribe(subscriber);

            ParameterizedThreadStart start = parameter =>
                                                 {
                                                     IEventAggregator ea =
                                                         parameter as IEventAggregator;

                                                     Assert.IsNotNull(ea);

                                                     ea.Publish(new SimpleMessage());
                                                 };

            Thread thread = new Thread(start);

            thread.Start(eventAggregator);

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void CannotSubscribeMultipleTime()
        {
            SynchronizationContext context = new SynchronizationContext();

            IEventAggregator eventAggregator = new EventAggregator(context);

            SimpleSubscriber subscriber = new SimpleSubscriber();

            eventAggregator.Subscribe(subscriber);

            //try to subscribe for the second time
            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void CanCancelMessageProcessing()
        {
            SynchronizationContext context = new SynchronizationContext();

            IEventAggregator eventAggregator = new EventAggregator(context);

            CancelingSubscriber subscriber = new CancelingSubscriber();

            eventAggregator.Subscribe(subscriber);

            ICancellationToken token = eventAggregator.PublishWithCancelOption(new CancelMessage());

            Assert.IsTrue(token.Cancel);
        }

        [TestMethod]
        public void CanUnsubscribe()
        {
            SynchronizationContext context = new SynchronizationContext();

            IEventAggregator eventAggregator = new EventAggregator(context);

            OneTimeSubscriber subscriber = new OneTimeSubscriber();

            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            eventAggregator.Unsubscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            Assert.AreEqual(1, subscriber.Counter);
        }
    }
}
