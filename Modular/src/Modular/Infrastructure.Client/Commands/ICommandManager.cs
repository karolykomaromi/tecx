namespace Infrastructure.Commands
{
    using System;

    public interface ICommandManager
    {
        event EventHandler RequerySuggested;

        void InvalidateRequerySuggested();
    }
}