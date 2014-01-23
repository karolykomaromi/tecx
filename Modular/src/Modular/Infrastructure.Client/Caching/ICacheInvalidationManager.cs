using System;

namespace Infrastructure.Caching
{
    public interface ICacheInvalidationManager
    {
        event EventHandler Invalidated;

        void RequestInvalidate();
    }
}