namespace Infrastructure.Commands
{
    using System;
    using System.Windows.Threading;
    using Infrastructure.Events;

    public class CommandManager : Alerting, ICommandManager
    {
        public CommandManager(Dispatcher dispatcher)
            : base(dispatcher)
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
