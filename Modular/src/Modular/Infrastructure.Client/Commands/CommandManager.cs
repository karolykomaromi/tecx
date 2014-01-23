namespace Infrastructure.Commands
{
    using System;
    using System.Windows.Threading;

    public class CommandManager : Alerting, ICommandManager
    {
        public CommandManager(Dispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public event EventHandler RequerySuggested
        {
            add { this.AddWeakReferenceHandler(value); }

            remove { this.RemoveWeakReferenceHandler(value); }
        }

        public void InvalidateRequerySuggested()
        {
            this.RaiseEvent();
        }
    }
}
