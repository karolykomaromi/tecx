using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace TecX.Common.Event
{
    public class EventAggregator : IEventAggregator
    {
        #region Fields

        // list to store all subscribers for ALL messages
        private List<WeakReference> _subscribers = new List<WeakReference>();

        // A Dispatcher for marshalling calls to the UI-Thread (update databound lists, etc.)
        private Dispatcher _dispatcher;

        // Thread-Synchronization locking object for access to Subscribers list
        private object _lock = new object();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the subscribers. For UnitTesting.
        /// </summary>
        /// <value>The subscribers.</value>
        internal IEnumerable<WeakReference> Subscribers
        {
            get
            {
                return _subscribers;
            }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        /// <param name="dispatcher">Dispatcher. Comes from App.xaml.cs</param>
        public EventAggregator(Dispatcher dispatcher)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");

            _dispatcher = dispatcher;

            // just one rough  possibility to purge dead subscriptions
            // runs every minute - needs to be adjusted for other apps.
            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            dispatcherTimer.Tick += RemoveDeadReferences;
            dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
            dispatcherTimer.Start();
        }

        #endregion c'tor

        #region Implementation of IEventAggregator

        /// <summary>
        /// Subscribes an object to the event aggregator.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Subscribe(object subscriber)
        {
            Guard.AssertNotNull(subscriber, "subscriber");

            RunLocked(() =>
            {
                if (IsNotNull(subscriber) &&
                    IsNotYetInCollection(subscriber) &&
                    IsHandlerInterfaceImplemented(subscriber))
                {
                    _subscribers.Add(new WeakReference(subscriber));
                }
                else
                {
                    throw new ArgumentException("Subscriber must implement ISubscribeTo<TMessage>.", "subscriber");
                }
            });
        }

        /// <summary>
        /// Unsubscribes an object from the event aggregator if registered.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe(object subscriber)
        {
            Guard.AssertNotNull(subscriber, "subscriber");

            RunLocked(() =>
            {
                var weakRefToRemove = (from item in _subscribers
                                       where item.Target == subscriber
                                       select item)
                        .FirstOrDefault();

                _subscribers.Remove(weakRefToRemove);
            });
        }

        /// <summary>
        /// Publishes a new message to all subsribers of this messagetype
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message containing data.</param>
        /// <returns></returns>
        public void Publish<TMessage>(TMessage message)
        {
            Guard.AssertNotNull(message, "message");

            RunLocked(() =>
            {
                foreach (var subscriber in GetAllSubscribersFor<TMessage>())
                {
                    if (_dispatcher != null)
                        _dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => subscriber.Handle(message)));
                    else
                        subscriber.Handle(message);
                }
            });
        }

        public ICancellationToken PublishWithCancelOption<TMessage>(TMessage message)
            where TMessage : ICancellationToken
        {
            Publish(message);

            return message;
        }

        #endregion Implementation of IEventAggregator

        #region Helper

        /// <summary>
        /// Gets all subscribers for a specific messagetype.
        /// </summary>
        /// <typeparam name="TMessage">The messagetype to search registered implementers for.</typeparam>
        /// <returns></returns>
        private IEnumerable<ISubscribeTo<TMessage>> GetAllSubscribersFor<TMessage>()
        {
            return (from subscriber in _subscribers
                    let handler = subscriber.Target as ISubscribeTo<TMessage>
                    where handler != null
                    select handler).ToList();
        }

        /// <summary>
        /// This is just to find wether an object subscribing is implementing the open generic version of IHandle<> 
        /// with any TMessage. It does not matter which TMessage exactly.
        /// </summary>
        /// <param name="subscriber">The subscribing object.</param>
        /// <returns>
        /// 	<c>true</c> if <see cref="ISubscribeTo{TMessage}"/>  is implemented; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsHandlerInterfaceImplemented(object subscriber)
        {
            Type[] interfaces = subscriber.GetType()
                    .FindInterfaces((type, criteria) =>
                    {
                        if (type.IsGenericType &&
                            type.GetGenericTypeDefinition() == typeof(ISubscribeTo<>))
                            return true;

                        return false;
                    }, null);

            return interfaces.Length > 0 ? true : false;
        }

        /// <summary>
        /// Runs all actions within a lock for thread-safe access to the subscribers list.
        /// ReaderWriterLock is a better choice than simple locking but it is more complex. 
        /// </summary>
        /// <param name="action">The action to execute within a lock.</param>
        private void RunLocked(Action action)
        {
            lock (_lock)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Determines whether the subscriber is null
        /// </summary>
        /// <param name="subscriber">The subscriber to check.</param>
        /// <returns>
        /// 	<c>true</c> if the subscriber is not null; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNotNull(object subscriber)
        {
            return subscriber != null ? true : false;
        }

        /// <summary>
        /// Determines whether the object is already in the collection
        /// </summary>
        /// <param name="subscriber">The subscriber to check for.</param>
        /// <returns>
        /// 	<c>true</c> if the subscriber is not registered yet; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNotYetInCollection(object subscriber)
        {
            var element = _subscribers.Select(o => o).Where(o => o.Target == subscriber).FirstOrDefault();
            return element == null ? true : false;
        }

        /// <summary>
        /// Removes registrations of diposed subscribers
        /// </summary>
        private void RemoveDeadReferences(object sender, EventArgs eventArgs)
        {
            RunLocked(() =>
            {
                var weakReferences = (from item in _subscribers
                                      where !item.IsAlive
                                      select item).ToList();

                weakReferences.ForEach(reference => _subscribers.Remove(reference));
            });
        }

        #endregion Helper
    }
}