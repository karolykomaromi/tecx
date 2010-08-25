using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TecX.Common.Event
{
    public class EventAggregator : IEventAggregator
    {
        #region Fields

        private readonly List<WeakReference> _subscribers;
        private readonly SynchronizationContext _context;
        private readonly object _syncRoot;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class
        /// </summary>
        public EventAggregator(SynchronizationContext context)
        {
            Guard.AssertNotNull(context, "context");

            _context = context;
            _subscribers = new List<WeakReference>();
            _syncRoot = new object();
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Implementation of IEventAggregator

        public void Subscribe(object subscriber)
        {
            Guard.AssertNotNull(subscriber, "subscriber");

            PurgeSubscribers();

            if (AllSubscribers()
                .Where(s => s.IsAlive && 
                    s.Target.Equals(subscriber))
                .Count() < 1)
            {
                WithinLock(() => _subscribers.Add(new WeakReference(subscriber)));
            }
        }

        public void Unsubscribe(object subscriber)
        {
            Guard.AssertNotNull(subscriber, "subscriber");

            PurgeSubscribers();

            var subscribed = AllSubscribers()
                                .Where(s => s.IsAlive &&
                                    s.Target.Equals(subscriber))
                                .FirstOrDefault();

            if (subscribed != null)
            {
                WithinLock(() => _subscribers.Remove(subscribed));
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            Guard.AssertNotNull(message, "message");

            foreach (WeakReference reference in _subscribers)
            {
                if(reference.IsAlive)
                {
                    ISubscribeTo<TMessage> subcriber = reference.Target as ISubscribeTo<TMessage>;

                    if(subcriber != null)
                    {
                        subcriber.Handle(message);
                    }
                }
            }
        }

        #endregion Implementation of IEventAggregator

        ////////////////////////////////////////////////////////////

        #region Helper

        private void WithinLock(Action action)
        {
            lock (_syncRoot)
            {
                action();
            }
        }

        private WeakReference[] AllSubscribers()
        {
            lock (_syncRoot)
            {
                return _subscribers.ToArray();
            }
        }

        private void PurgeSubscribers()
        {
            var dead = from subscriber in AllSubscribers()
                       where !subscriber.IsAlive
                       select subscriber;

            WithinLock(() =>
                           {
                               foreach (WeakReference reference in dead)
                               {
                                   _subscribers.Remove(reference);
                               }
                           });
        }

        #endregion Helper

    }
}
