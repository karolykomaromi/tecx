using System;

namespace Infrastructure.Commands
{
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