namespace Infrastructure.Commands
{
    using System;
    using System.Windows.Threading;
    using Infrastructure.Events;
    using Microsoft.Practices.Prism.Logging;

    public class CommandManager : Alerting, ICommandManager
    {
        public CommandManager(Dispatcher dispatcher, ILoggerFacade logger)
            : base(dispatcher, logger)
        {
        }

        public event EventHandler RequerySuggested
        {
            add { this.AddHandler(value); }

            remove { this.RemoveHandler(value); }
        }

        public void InvalidateRequerySuggested()
        {
            this.RaiseEvent();
        }
    }
}
