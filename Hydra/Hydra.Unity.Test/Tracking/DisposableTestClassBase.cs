namespace Hydra.Unity.Test.Tracking
{
    using System;

    public class DisposableTestClassBase : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}