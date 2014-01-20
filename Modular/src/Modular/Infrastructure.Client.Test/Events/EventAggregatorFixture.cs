using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Infrastructure.Events;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Client.Test.Events
{
    [TestClass]
    public class EventAggregatorFixture : SilverlightTest
    {
        [TestMethod]
        [Asynchronous]
        public void Should_Successfully_Publish_Message_On_Same_Thread()
        {
            bool done = false;

            Dispatcher dispatcher = Deployment.Current.Dispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CountingSubscriber subscriber = new CountingSubscriber(() => { done = true; });

            eventAggregator.Subscribe(subscriber);
            eventAggregator.Publish(new Message());

            EnqueueConditional(() => done);
            EnqueueCallback(() => Assert.AreEqual(1, subscriber.MessageCounter));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void Should_Successfully_Publish_Message_On_Different_Thread()
        {
            bool done = false;

            Dispatcher dispatcher = Deployment.Current.Dispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CountingSubscriber subscriber = new CountingSubscriber(() => { done = true; });

            eventAggregator.Subscribe(subscriber);

            ParameterizedThreadStart start = parameter =>
            {
                IEventAggregator ea = (IEventAggregator)parameter;
                ea.Publish(new Message());
            };

            Thread thread = new Thread(start);
            thread.Start(eventAggregator);

            EnqueueConditional(() => done);
            EnqueueCallback(() => Assert.AreEqual(1, subscriber.MessageCounter));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void Should_Ignore_Repeated_Subscription_Of_Same_Handler()
        {
            bool done = false;

            Dispatcher dispatcher = Deployment.Current.Dispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CountingSubscriber subscriber = new CountingSubscriber(() => { done = true; });

            eventAggregator.Subscribe(subscriber);
            eventAggregator.Subscribe(subscriber);

            eventAggregator.Publish(new Message());

            EnqueueConditional(() => done);
            EnqueueCallback(() => Assert.AreEqual(1, subscriber.MessageCounter));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void Should_Return_Message_Cancelled_By_Subscriber()
        {
            bool done = false;

            Dispatcher dispatcher = Deployment.Current.Dispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CancelingSubscriber subscriber = new CancelingSubscriber(() => { done = true; });

            eventAggregator.Subscribe(subscriber);

            CancelMessage token = eventAggregator.Publish(new CancelMessage());

            EnqueueConditional(() => done);
            EnqueueCallback(() => Assert.IsTrue(token.Cancel));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void Should_Not_Publish_Message_To_Unsubscribed_Handler()
        {
            Dispatcher dispatcher = Deployment.Current.Dispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            CountingSubscriber subscriber = new CountingSubscriber(() => { });

            eventAggregator.Subscribe(subscriber);
            eventAggregator.Unsubscribe(subscriber);

            eventAggregator.Publish(new Message());

            EnqueueDelay(TimeSpan.FromSeconds(2));
            EnqueueCallback(() => Assert.AreEqual(0, subscriber.MessageCounter));
            EnqueueTestComplete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_Exception_When_Subscriber_Does_Not_Implement_Handler_Interface()
        {
            Dispatcher dispatcher = Deployment.Current.Dispatcher;

            IEventAggregator eventAggregator = new EventAggregator(dispatcher);

            eventAggregator.Subscribe(new object());
        }
    }

    public class CountingSubscriber : ISubscribeTo<Message>
    {
        public int MessageCounter = 0;

        private readonly Action action;

        public CountingSubscriber(Action action)
        {
            this.action = action;
        }

        public void Handle(Message message)
        {
            Interlocked.Increment(ref MessageCounter);

            action();
        }
    }

    public class Message
    {
    }

    public class CancelingSubscriber : ISubscribeTo<CancelMessage>
    {
        private readonly Action action;

        public CancelingSubscriber(Action action)
        {
            this.action = action;
        }

        public void Handle(CancelMessage message)
        {
            message.Cancel = true;
            action();
        }
    }

    public class CancelMessage
    {
        public bool Cancel { get; set; }
    }
}
