namespace TecX.Agile.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Threading;

    using TecX.Common;
    using TecX.Common.Extensions.Primitives;

    public abstract class MessageChannel : IMessageChannel
    {
        private readonly List<WeakReference> subscribers = new List<WeakReference>();

        private readonly Dispatcher dispatcher;

        private readonly object locker = new object();

        protected MessageChannel(Dispatcher dispatcher)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");

            this.dispatcher = dispatcher;

            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);

            dispatcherTimer.Tick += this.RemoveDeadReferences;
            dispatcherTimer.Interval = 1.Minutes();
            dispatcherTimer.Start();
        }

        public void Subscribe(object subscriber)
        {
            Guard.AssertNotNull(subscriber, "subscriber");

            this.RunLocked(() =>
            {
                if (IsAlreadyInCollection(subscriber))
                {
                    return;
                }

                AssertHandlerInterfaceImplemented(subscriber);

                this.subscribers.Add(new WeakReference(subscriber));
            });
        }

        public void Unsubscribe(object subscriber)
        {
            Guard.AssertNotNull(subscriber, "subscriber");

            this.RunLocked(() =>
            {
                var weakRefToRemove = (from item in this.subscribers
                                       where item.Target == subscriber
                                       select item)
                        .FirstOrDefault();

                this.subscribers.Remove(weakRefToRemove);
            });
        }

        public abstract void Send<TMessage>(TMessage message) where TMessage : class;

        protected virtual void NotifySubscribers<TMessage>(TMessage message)
            where TMessage : class
        {
            Guard.AssertNotNull(message, "message");

            this.RunLocked(() =>
            {
                foreach (var subscriber in GetAllSubscribersFor<TMessage>())
                {
                    if (this.dispatcher != null)
                    {
                        this.dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => subscriber.Handle(message)));
                    }
                    else
                    {
                        subscriber.Handle(message);
                    }
                }
            });
        }

        private static void AssertHandlerInterfaceImplemented(object subscriber)
        {
            Type[] interfaces = subscriber.GetType().FindInterfaces(
                (type, criteria) =>
                {
                    if (type.IsGenericType &&
                        type.GetGenericTypeDefinition() == typeof(IMessageSubscriber<>))
                    {
                        return true;
                    }

                    return false;
                },
                    null);

            if (interfaces.Length == 0)
            {
                throw new ArgumentException("Subscriber must implement IMessageSubscriber<TMessage>.", "subscriber");
            }
        }

        private IEnumerable<IMessageSubscriber<TMessage>> GetAllSubscribersFor<TMessage>()
        {
            return (from subscriber in this.subscribers
                    let handler = subscriber.Target as IMessageSubscriber<TMessage>
                    where handler != null
                    select handler).ToList();
        }

        private void RunLocked(Action action)
        {
            lock (this.locker)
            {
                action.Invoke();
            }
        }

        private bool IsAlreadyInCollection(object subscriber)
        {
            var element = this.subscribers.Select(o => o).FirstOrDefault(o => o.Target == subscriber);

            return element != null;
        }

        private void RemoveDeadReferences(object sender, EventArgs eventArgs)
        {
            this.RunLocked(() =>
            {
                var weakReferences = (from item in this.subscribers
                                      where !item.IsAlive
                                      select item).ToList();

                weakReferences.ForEach(reference => this.subscribers.Remove(reference));
            });
        }
    }
}