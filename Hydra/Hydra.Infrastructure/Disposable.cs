namespace Hydra.Infrastructure
{
    using System;

    public class Disposable : IDisposable
    {
        public static readonly IDisposable Empty = new Disposable();

        private Disposable()
        {
        }

        public void Dispose()
        {
        }
    }
}