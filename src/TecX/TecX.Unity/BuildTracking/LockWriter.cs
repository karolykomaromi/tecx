using System;
using System.Threading;

using TecX.Common;

namespace TecX.Unity.BuildTracking
{
    /// <summary>
    /// The <see cref="LockWriter"/>
    ///   class is used to provide thread safe write access to a resource using a provided
    ///   <see cref="ReaderWriterLock"/> or <see cref="ReaderWriterLockSlim"/> instance.
    /// </summary>
    public sealed class LockWriter : IDisposable
    {
        /// <summary>
        /// Gets or sets the cookie.
        /// </summary>
        /// <value>
        /// The cookie.
        /// </value>
        private readonly LockCookie _cookie;

        /// <summary>
        /// Gets or sets the lock reference.
        /// </summary>
        /// <value>
        /// The lock reference.
        /// </value>
        private readonly ReaderWriterLock _lockReference;

        /// <summary>
        /// Gets or sets the lock reference slim.
        /// </summary>
        /// <value>
        /// The lock reference slim.
        /// </value>
        private readonly ReaderWriterLockSlim _lockReferenceSlim;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LockWriter"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        private Boolean Disposed
        {
            get;
            set;
        }

        /// <overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="LockWriter"/> class.
        ///   </summary>
        /// </overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="LockWriter"/> class using the provided lock reference.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        public LockWriter(ReaderWriterLockSlim lockReference)
            : this(lockReference, Timeout.Infinite)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockWriter"/> class using the provided lock reference and timeout value.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="timeout">
        /// The timeout value within which the lock must be entered, measured in milliseconds.
        /// </param>
        public LockWriter(ReaderWriterLockSlim lockReference, Int32 timeout)
        {
            Guard.AssertNotNull(lockReference, "lockReference");

            _lockReferenceSlim = lockReference;

            // Acquire a write lock
            if (_lockReferenceSlim.TryEnterWriteLock(timeout) == false)
            {
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockWriter"/> class using the provided lock reference.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        public LockWriter(ReaderWriterLock lockReference)
            : this(lockReference, Timeout.Infinite)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockWriter"/> class using the provided lock reference and timeout value.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="timeout">
        /// The timeout value within which the lock must be entered, measured in milliseconds.
        /// </param>
        public LockWriter(ReaderWriterLock lockReference, Int32 timeout)
        {
            Guard.AssertNotNull(lockReference, "lockReference");

            _cookie = default(LockCookie);

            _lockReference = lockReference;

            // Check if there is already read lock on this thread
            // IMPORTANT: This is to avoid deadlocks on the current thread
            // See Remarks (caution) on ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/fxref_mscorlib/html/9e17065a-0b8b-f611-1166-63231b0c6987.htm
            if (_lockReference.IsReaderLockHeld)
            {
                try
                {
                    // There is already a read lock on the thread
                    // Upgrade the reader lock to a writer lock
                    _cookie = _lockReference.UpgradeToWriterLock(timeout);
                }
                catch (ApplicationException ex)
                {
                    // Wrap the ReaderWriterLocks native timeout exception (ApplicationException) in an actual TimeoutException
                    throw new TimeoutException(ex.Message, ex);
                }
            }
            else
            {
                try
                {
                    // Acquire a write lock
                    _lockReference.AcquireWriterLock(timeout);
                }
                catch (ApplicationException ex)
                {
                    // Wrap the ReaderWriterLocks native timeout exception (ApplicationException) in an actual TimeoutException
                    throw new TimeoutException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// The <see cref="Dispose"/> method releases the lock that was acquired by this instance. 
        ///   If an upgraded lock was obtained, the write lock is downgraded to a read lock.
        /// </remarks>
        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;

            if (_lockReferenceSlim != null)
            {
                if (_lockReferenceSlim.IsWriteLockHeld)
                {
                    _lockReferenceSlim.ExitWriteLock();
                }
            }
            else
            {
                if (_lockReference.IsWriterLockHeld)
                {
                    // Check if there was previously a reader lock
                    if (_cookie == default(LockCookie))
                    {
                        // Release the write lock
                        _lockReference.ReleaseWriterLock();
                    }
                    else
                    {
                        LockCookie internalCookie = _cookie;

                        // Downgrade the writer lock back to a reader lock
                        _lockReference.DowngradeFromWriterLock(ref internalCookie);
                    }
                }
            }
        }
    }
}