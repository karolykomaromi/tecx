﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Infrastructure.Events
{
    public class EventAggregator : IEventAggregator
    {
        private static readonly Lazy<IEventAggregator> _Instance = new Lazy<IEventAggregator>(() => new EventAggregator(Deployment.Current.Dispatcher));

        // list to store all subscribers for ALL messages
        private readonly List<WeakReference> _Subscribers = new List<WeakReference>();

        // A Dispatcher for marshalling calls to the UI-Thread (update databound lists, etc.)
        private readonly Dispatcher _Dispatcher;

        // Thread-Synchronization locking object for access to Subscribers list
        private readonly object _Locker = new object();

        public static IEventAggregator Instance
        {
            get { return _Instance.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        /// <param name="dispatcher"> Dispatcher. Comes from App.xaml.cs</param>
        public EventAggregator(Dispatcher dispatcher)
        {
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            this._Dispatcher = dispatcher;

            // just one rough possibility to purge dead subscriptions
            // runs every minute - needs to be adjusted for other apps.
            var dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += this.RemoveDeadReferences;
            dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Subscribes an object to the event aggregator.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Subscribe(object subscriber)
        {
            this.RunLocked(() =>
                {
                    if (IsAlreadyInCollection(subscriber))
                    {
                        return;
                    }

                    AssertHandlerInterfaceImplemented(subscriber);

                    this._Subscribers.Add(new WeakReference(subscriber));
                });
        }

        /// <summary>
        /// Unsubscribes an object from the event aggregator if registered.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe(object subscriber)
        {
            this.RunLocked(() =>
                {
                    var weakRefToRemove = (from item in this._Subscribers
                                           where item.Target == subscriber
                                           select item)
                        .FirstOrDefault();

                    this._Subscribers.Remove(weakRefToRemove);
                });
        }

        /// <summary>
        /// Publishes a new message to all subsribers of this messagetype
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message containing data.</param>
        /// <returns>The <paramref name="message"/></returns>
        public TMessage Publish<TMessage>(TMessage message) where TMessage : class
        {
            this.RunLocked(
                () =>
                    {
                        foreach (var subscriber in GetAllSubscribersFor<TMessage>())
                        {
                            if (this._Dispatcher != null)
                            {
                                ISubscribeTo<TMessage> sub = subscriber;

                                this._Dispatcher.BeginInvoke(() => sub.Handle(message));
                            }
                            else
                            {
                                subscriber.Handle(message);
                            }
                        }
                    });

            return message;
        }

        /// <summary>
        /// This is just to find wether an object subscribing is implementing the open generic version of IHandle<> 
        /// with any TMessage. It does not matter which TMessage exactly.
        /// </summary>
        /// <param name="subscriber">The subscribing object.</param>
        /// <returns>
        ///     <c>true</c> if <see cref="ISubscribeTo{TMessage}"/> is implemented; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets all subscribers for a specific messagetype.
        /// </summary>
        /// <typeparam name="TMessage">The messagetype to search registered implementers for.</typeparam>
        /// <returns>List of all subscribers for <typeparamref name="TMessage"/></returns>
        private IEnumerable<ISubscribeTo<TMessage>> GetAllSubscribersFor<TMessage>()
        {
            return (from subscriber in this._Subscribers
                    let handler = subscriber.Target as ISubscribeTo<TMessage>
                    where handler != null
                    select handler).ToList();
        }

        /// <summary>
        /// Runs all actions within a lock for thread-safe access to the subscribers list.
        /// ReaderWriterLock is a better choice than simple locking but it is more complex. 
        /// </summary>
        /// <param name="action">The action to execute within a lock.</param>
        private void RunLocked(Action action)
        {
            lock (this._Locker)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Determines whether the object is already in the collection
        /// </summary>
        /// <param name="subscriber">The subscriber to check for.</param>
        /// <returns>
        ///     <c>true</c> if the subscriber is not registered yet; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAlreadyInCollection(object subscriber)
        {
            var element = this._Subscribers.Select(o => o).FirstOrDefault(o => o.Target == subscriber);

            return element != null;
        }

        /// <summary>
        /// Removes registrations of diposed subscribers
        /// </summary>
        private void RemoveDeadReferences(object sender, EventArgs eventArgs)
        {
            this.RunLocked(() =>
                {
                    var weakReferences = (from item in this._Subscribers
                                          where !item.IsAlive
                                          select item).ToList();

                    weakReferences.ForEach(reference => this._Subscribers.Remove(reference));
                });
        }
    }
}