using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Event;

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

            ParameterizedThreadStart start = new ParameterizedThreadStart(parameter =>
                                                                              {
                                                                                  IEventAggregator ea =
                                                                                      parameter as IEventAggregator;

                                                                                  Assert.IsNotNull(ea);

                                                                                  ea.Publish(new SimpleMessage());
                                                                              });

            start.Invoke(eventAggregator);

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

    class OneTimeSubscriber : ISubscribeTo<SimpleMessage>
    {
        public int Counter = 0;

        public void Handle(SimpleMessage message)
        {
            Interlocked.Increment(ref Counter);

            if (Counter == 2)
                Assert.Fail("must not be called twice");
        }
    }

    class CancelingSubscriber : ISubscribeTo<CancelMessage>
    {
        #region Implementation of ISubscribeTo<in CancelMessage>

        public void Handle(CancelMessage message)
        {
            message.Cancel = true;
        }

        #endregion
    }

    class CancelMessage : ICancellationToken
    {
        public bool Cancel { get; set; }
    }

    class SimpleSubscriber : ISubscribeTo<SimpleMessage>
    {
        public bool MessageReceived = false;
        public int MessageCounter = 0;

        public void Handle(SimpleMessage message)
        {
            Assert.IsNotNull(message);

            MessageReceived = true;
            Interlocked.Increment(ref MessageCounter);
        }
    }

    class SimpleMessage
    {
    }
}
