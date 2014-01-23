namespace Infrastructure.Caching
{
    using System;

    public interface ICacheInvalidationManager
    {
        event EventHandler Invalidated;

        void RequestInvalidate();
    }
}