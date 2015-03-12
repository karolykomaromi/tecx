namespace Hydra.Unity.Test.Tracking
{
    using System;

    public interface IDisposeThis : IDisposable
    {
        bool IsDisposed { get; }
    }
}