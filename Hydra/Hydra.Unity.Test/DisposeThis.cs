namespace Hydra.Unity.Test
{
    using System;

    public class DisposeThis : IDisposable
    {
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}
