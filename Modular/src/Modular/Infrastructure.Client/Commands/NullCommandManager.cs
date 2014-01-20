namespace Infrastructure.Commands
{
    using System;

    public class NullCommandManager : ICommandManager
    {
        public event EventHandler RequerySuggested
        {
            add { }
            remove { }
        }

        public void InvalidateRequerySuggested()
        {
        }
    }
}