namespace Infrastructure.Events
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Windows.Threading;
    using Microsoft.Practices.Prism.Logging;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Generic and non-generic version of same class.")]
    public abstract class Alerting
    {
        private static int ActiveAlertsCounter = 0;

        private readonly EventHandlerList eventHandlers;
        private readonly Dispatcher dispatcher;
        private readonly ILoggerFacade logger;
        private bool hasCanExecuteQueued;

        protected Alerting(Dispatcher dispatcher, ILoggerFacade logger)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(logger != null);

            this.dispatcher = dispatcher;
            this.logger = logger;
            this.eventHandlers = new EventHandlerList();
        }

        protected void RaiseEvent()
        {
            if (this.hasCanExecuteQueued)
            {
                return;
            }

            this.hasCanExecuteQueued = true;

            Interlocked.Increment(ref ActiveAlertsCounter);

            string msg = string.Format("Enqueing new alert for '{0}'. Alerts currently waiting in queue: '{1}'", this.GetType().FullName, ActiveAlertsCounter);

            this.logger.Log(msg, Category.Debug, Priority.None);

            this.dispatcher.BeginInvoke(
                () =>
                {
                    this.CallHandlers();
                    this.hasCanExecuteQueued = false;
                    Interlocked.Decrement(ref ActiveAlertsCounter);
                    string msg1 = string.Format("Alert for '{0}' handled. Alerts currently waiting in queue: '{1}'", this.GetType().FullName, ActiveAlertsCounter);
                    this.logger.Log(msg1, Category.Debug, Priority.None);
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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Generic and non-generic version of same class.")]
    public abstract class Alerting<T> where T : EventArgs
    {
        private static int ActiveAlertsCounter = 0;
        private readonly EventHandlerList<T> eventHandlers;
        private readonly Dispatcher dispatcher;
        private readonly ILoggerFacade logger;
        private bool hasCanExecuteQueued;

        protected Alerting(Dispatcher dispatcher, ILoggerFacade logger)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
            this.logger = logger;
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

            Interlocked.Increment(ref ActiveAlertsCounter);

            string msg = string.Format("Enqueing new alert for '{0}'. Alerts currently waiting in queue: '{1}'", this.GetType().FullName, ActiveAlertsCounter);

            this.logger.Log(msg, Category.Debug, Priority.None);

            this.dispatcher.BeginInvoke(
                () =>
                {
                    this.CallHandlers(args);
                    this.hasCanExecuteQueued = false;
                    Interlocked.Decrement(ref ActiveAlertsCounter);
                    string msg1 = string.Format("Alert for '{0}' handled. Alerts currently waiting in queue: '{1}'", this.GetType().FullName, ActiveAlertsCounter);
                    this.logger.Log(msg1, Category.Debug, Priority.None);
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