using System;
using System.Threading;

namespace Hydra.Unity.Tracking
{
    public class UpgradeableReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rws;

        private bool isDisposed;

        public UpgradeableReadLock(ReaderWriterLockSlim rws, int timeoutInMilliseconds = Timeout.Infinite)
            : this(rws, TimeSpan.FromMilliseconds(timeoutInMilliseconds))
        {
        }

        private UpgradeableReadLock(ReaderWriterLockSlim rws, TimeSpan timeout)
        {
            this.rws = rws;

            if (!this.rws.TryEnterUpgradeableReadLock(timeout))
            {
                throw new TimeoutException();
            }
        }

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.rws.IsUpgradeableReadLockHeld)
            {
                this.rws.ExitUpgradeableReadLock();
            }

            this.isDisposed = true;
        }
    }
}