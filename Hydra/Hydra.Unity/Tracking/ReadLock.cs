namespace Hydra.Unity.Tracking
{
    using System;
    using System.Threading;

    public class ReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rws;

        private bool isDisposed;

        public ReadLock(ReaderWriterLockSlim rws, int timeoutInMilliseconds = 500)
            : this(rws, TimeSpan.FromMilliseconds(timeoutInMilliseconds))
        {
        }

        public ReadLock(ReaderWriterLockSlim rws, TimeSpan timeout)
        {
            this.rws = rws;

            if (!rws.TryEnterReadLock(timeout))
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
            if (this.isDisposed)
            {
                return;
            }

            if (this.rws.IsReadLockHeld)
            {
                this.rws.ExitReadLock();
            }

            this.isDisposed = true;
        }
    }
}