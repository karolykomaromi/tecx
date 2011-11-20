using System;
using System.Threading;
using System.Windows.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Event;
using TecX.Common.Test.TestObjects;
using TecX.TestTools;

namespace TecX.Common.Test
{
    [TestClass]
    public class EventAggregatorFixture
    {
        [TestMethod]
        public void CanPublishMessageOnSameThread()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            SimpleSubscriber subscriber = new SimpleSubscriber();

            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            DispatcherUtil.DoEvents();

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void CanPublishMessageOnDifferentThread()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            SimpleSubscriber subscriber = new SimpleSubscriber();

            eventAggregator.Subscribe(subscriber);

            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            ParameterizedThreadStart start = parameter =>
                                                 {
                                                     IEventAggregator ea =
                                                         parameter as IEventAggregator;

                                                     Assert.IsNotNull(ea);

                                                     ea.Publish(new SimpleMessage());

                                                     manualResetEvent.Set();
                                                 };

            Thread thread = new Thread(start);

            thread.Start(eventAggregator);

            manualResetEvent.WaitOne(1000, false);

            DispatcherUtil.DoEvents();

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void WhenSubscribingMultipleTimes_RepeatedSubscriptionsAreIgnored()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            SimpleSubscriber subscriber = new SimpleSubscriber();

            eventAggregator.Subscribe(subscriber);

            //try to subscribe for the second time
            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            DispatcherUtil.DoEvents();

            Assert.IsTrue(subscriber.MessageReceived);
            Assert.AreEqual(1, subscriber.MessageCounter);
        }

        [TestMethod]
        public void WhenPublishingWithCancelOption_CancellationTokenIsReturned()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CancelingSubscriber subscriber = new CancelingSubscriber();

            eventAggregator.Subscribe(subscriber);

            CancelMessage token = eventAggregator.Publish(new CancelMessage());

            DispatcherUtil.DoEvents();

            Assert.IsTrue(token.Cancel);
        }

        [TestMethod]
        public void WhenUnsubscribing_SubscriberIsRemoved()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            OneTimeSubscriber subscriber = new OneTimeSubscriber();

            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());

            eventAggregator.Unsubscribe(subscriber);

            eventAggregator.Publish(new SimpleMessage());


            DispatcherUtil.DoEvents();

            Assert.AreEqual(1, subscriber.Counter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenSubscribingObjectThatDoesNotImplementHandlerInterface_ExceptionIsThrown()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            eventAggregator.Subscribe(new object());
        }
    }
}
