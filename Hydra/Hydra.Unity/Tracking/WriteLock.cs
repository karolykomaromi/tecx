namespace Hydra.Unity.Tracking
{
    using System;
    using System.Threading;

    public class WriteLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rws;

        private bool isDisposed;

        public WriteLock(ReaderWriterLockSlim rws, int timeoutInMilliseconds = Timeout.Infinite)
            : this(rws, TimeSpan.FromMilliseconds(timeoutInMilliseconds))
        {
        }

        public WriteLock(ReaderWriterLockSlim rws, TimeSpan timeout)
        {
            this.rws = rws;

            if (!rws.TryEnterWriteLock(timeout))
            {
                throw new TimeoutException();
            }
        }

        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.rws.IsWriteLockHeld)
            {
                this.rws.ExitWriteLock();
            }

            this.isDisposed = true;
        }
    }
}