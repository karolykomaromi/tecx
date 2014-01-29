namespace Infrastructure.Events
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Generic and non-generic version of same class.")]
    public class EventHandlerList : IEnumerable<EventHandler>
    {
        private readonly List<WeakReference> eventHandlers;

        public EventHandlerList()
        {
            this.eventHandlers = new List<WeakReference>();
        }

        public void AddHandler(EventHandler handler)
        {
            Contract.Requires(handler != null);

            this.eventHandlers.Add(new WeakReference(handler));
        }

        public void RemoveHandler(EventHandler handler)
        {
            Contract.Requires(handler != null);

            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];

                EventHandler target = reference.Target as EventHandler;

                if (target == null || target == handler)
                {
                    this.eventHandlers.RemoveAt(i);
                }
            }
        }

        public IEnumerator<EventHandler> GetEnumerator()
        {
            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];

                EventHandler handler = reference.Target as EventHandler;

                if (handler == null)
                {
                    this.eventHandlers.RemoveAt(i);
                }
                else
                {
                    yield return handler;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Generic and non-generic version of same class.")]
    public class EventHandlerList<T> : IEnumerable<EventHandler<T>> where T : EventArgs
    {
        private readonly List<WeakReference> eventHandlers;

        public EventHandlerList()
        {
            this.eventHandlers = new List<WeakReference>();
        }

        public void AddHandler(EventHandler<T> handler)
        {
            Contract.Requires(handler != null);

            this.eventHandlers.Add(new WeakReference(handler));
        }

        public void RemoveHandler(EventHandler<T> handler)
        {
            Contract.Requires(handler != null);

            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];

                EventHandler<T> target = reference.Target as EventHandler<T>;

                if (target == null || target == handler)
                {
                    this.eventHandlers.RemoveAt(i);
                }
            }
        }

        public IEnumerator<EventHandler<T>> GetEnumerator()
        {
            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];

                EventHandler<T> handler = reference.Target as EventHandler<T>;

                if (handler == null)
                {
                    this.eventHandlers.RemoveAt(i);
                }
                else
                {
                    yield return handler;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}