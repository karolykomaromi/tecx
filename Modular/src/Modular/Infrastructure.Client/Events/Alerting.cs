namespace Infrastructure.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Threading;

    public abstract class Alerting
    {
        private readonly EventHandlerList eventHandlers;
        private readonly Dispatcher dispatcher;
        private bool hasCanExecuteQueued;

        protected Alerting(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
            this.eventHandlers = new EventHandlerList();
        }

        protected void RaiseEvent()
        {
            if (this.hasCanExecuteQueued)
            {
                return;
            }

            this.hasCanExecuteQueued = true;

            this.dispatcher.BeginInvoke(
                () =>
                {
                    this.CallHandlers();
                    this.hasCanExecuteQueued = false;
                });
        }

        protected void RemoveHandler(EventHandler handler)
        {
            Contract.Requires(handler != null);

            this.eventHandlers.RemoveHandler(handler);
        }

        protected void AddHandler(EventHandler handler)
        {
            Contract.Requires(handler != null);
            this.eventHandlers.AddHandler(handler);
        }

        private void CallHandlers()
        {
            foreach (EventHandler handler in this.eventHandlers)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    public abstract class Alerting<T>
        where T : EventArgs
    {
        private readonly EventHandlerList<T> eventHandlers;
        private readonly Dispatcher dispatcher;
        private bool hasCanExecuteQueued;

        protected Alerting(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
            this.eventHandlers = new EventHandlerList<T>();
        }

        protected void RaiseEvent(T args)
        {
            Contract.Requires(args != null);

            if (this.hasCanExecuteQueued)
            {
                return;
            }

            this.hasCanExecuteQueued = true;

            this.dispatcher.BeginInvoke(
                () =>
                {
                    this.CallHandlers(args);
                    this.hasCanExecuteQueued = false;
                });
        }

        protected void RemoveHandler(EventHandler<T> handler)
        {
            Contract.Requires(handler != null);

            this.eventHandlers.RemoveHandler(handler);
        }

        protected void AddHandler(EventHandler<T> handler)
        {
            Contract.Requires(handler != null);

            this.eventHandlers.AddHandler(handler);
        }

        private void CallHandlers(T args)
        {
            foreach (EventHandler<T> handler in this.eventHandlers)
            {
                handler(this, args);
            }
        }
    }
}