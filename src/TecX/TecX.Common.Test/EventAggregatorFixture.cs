using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Event;
using TecX.Common.Test.TestClasses;
using System.Windows.Threading;
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
        public void CannotSubscribeMultipleTime()
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
        public void CanCancelMessageProcessing()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CancelingSubscriber subscriber = new CancelingSubscriber();

            eventAggregator.Subscribe(subscriber);

            ICancellationToken token = eventAggregator.PublishWithCancelOption(new CancelMessage());

            DispatcherUtil.DoEvents();

            Assert.IsTrue(token.Cancel);
        }

        [TestMethod]
        public void CanUnsubscribe()
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
    }
}
