using System;
using System.Threading;

namespace Hydra.Unity.Tracking
{
    public class ReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rws;

        private bool isDisposed;

        public ReadLock(ReaderWriterLockSlim rws, int timeoutInMilliseconds = Timeout.Infinite)
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