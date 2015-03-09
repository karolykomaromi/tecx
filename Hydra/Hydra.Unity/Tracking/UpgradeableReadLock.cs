namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;

    public class UpgradeableReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim rws;

        private bool isDisposed;

        public UpgradeableReadLock(ReaderWriterLockSlim rws, int timeoutInMilliseconds = 500)
            : this(rws, TimeSpan.FromMilliseconds(timeoutInMilliseconds))
        {
            Contract.Requires(rws != null);
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
            get { return this.isDisposed; }
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