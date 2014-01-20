using System;

namespace Infrastructure.Commands
{
    public interface ICommandManager
    {
        event EventHandler RequerySuggested;
        void InvalidateRequerySuggested();
    }
}