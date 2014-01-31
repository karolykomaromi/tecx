namespace Infrastructure.Events
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows.Threading;
    using Microsoft.Practices.ServiceLocation;

    public class EventAggregator : IEventAggregator
    {
        private static readonly Lazy<IEventAggregator> Lazy = new Lazy<IEventAggregator>(() => ServiceLocator.Current.GetInstance<IEventAggregator>());

        private readonly List<WeakReference> subscribers = new List<WeakReference>();
        private readonly Dispatcher dispatcher;
        private readonly object locker = new object();

        public EventAggregator(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;

            var dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += this.RemoveDeadReferences;
            dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
            dispatcherTimer.Start();
        }

        public static IEventAggregator Current
        {
            get { return Lazy.Value; }
        }

        public void Subscribe(object subscriber)
        {
            this.RunLocked(() =>
                {
                    if (this.IsAlreadyInCollection(subscriber))
                    {
                        return;
                    }

                    AssertHandlerInterfaceImplemented(subscriber);

                    this.subscribers.Add(new WeakReference(subscriber));
                });
        }

        public void Unsubscribe(object subscriber)
        {
            this.RunLocked(() =>
                {
                    var weakRefToRemove = (from item in this.subscribers
                                           where item.Target == subscriber
                                           select item)
                        .FirstOrDefault();

                    this.subscribers.Remove(weakRefToRemove);
                });
        }

        public TMessage Publish<TMessage>(TMessage message) where TMessage : class
        {
            this.RunLocked(
                () =>
                    {
                        foreach (var subscriber in this.GetAllSubscribersFor<TMessage>())
                        {
                            ISubscribeTo<TMessage> sub = subscriber;
                            this.dispatcher.BeginInvoke(() => sub.Handle(message));
                        }
                    });

            return message;
        }

        private static void AssertHandlerInterfaceImplemented(object subscriber)
        {
            Type[] interfaces = subscriber.GetType().FindInterfaces(
                (type, criteria) =>
                    {
                        if (type.IsGenericType &&
                            type.GetGenericTypeDefinition() == typeof(ISubscribeTo<>))
                        {
                            return true;
                        }

                        return false;
                    },
                null);

            if (interfaces.Length == 0)
            {
                throw new ArgumentException("Subscriber must implement ISubscribeTo<TMessage>.", "subscriber");
            }
        }

        private IEnumerable<ISubscribeTo<TMessage>> GetAllSubscribersFor<TMessage>()
        {
            return (from subscriber in this.subscribers
                    let handler = subscriber.Target as ISubscribeTo<TMessage>
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