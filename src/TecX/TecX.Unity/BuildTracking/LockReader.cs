using System;
using System.Threading;

using TecX.Common;

namespace TecX.Unity.BuildTracking
{
    /// <summary>
    /// The <see cref="LockReader"/>
    ///   class is used to provide thread safe read access to a resource using a provided 
    ///   <see cref="ReaderWriterLock"/> or <see cref="ReaderWriterLockSlim"/> instance.
    /// </summary>
    public sealed class LockReader : IDisposable
    {
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
        /// Gets or sets a value indicating whether an upgradable lock was requested.
        /// </summary>
        /// <value>
        /// <c>true</c> if an upgradable lock was requested; otherwise, <c>false</c>.
        /// </value>
        private readonly Boolean _upgradableLockRequested;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LockReader"/> is disposed.
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
        /// Initializes a new instance of the <see cref="LockReader"/> class.
        ///   </summary>
        /// </overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="LockReader"/> class using the provided lock reference.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        public LockReader(ReaderWriterLockSlim lockReference)
            : this(lockReference, Timeout.Infinite, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockReader"/> class using the provided lock reference and timeout value.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="timeout">
        /// The timeout value within which the lock must be entered, measured in milliseconds.
        /// </param>
        public LockReader(ReaderWriterLockSlim lockReference, int timeout)
            : this(lockReference, timeout, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockReader"/> class using the provided lock reference and lock upgrade flag.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="obtainUpgradeableLock">
        /// <c>true</c> to obtain an upgradeable lock; otherwise <c>false</c>.
        /// </param>
        public LockReader(ReaderWriterLockSlim lockReference, bool obtainUpgradeableLock)
            : this(lockReference, Timeout.Infinite, obtainUpgradeableLock)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockReader"/> class using the provided lock reference, timeout value and lock upgrade flag.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="timeout">
        /// The timeout value within which the lock must be entered, measured in milliseconds.
        /// </param>
        /// <param name="obtainUpgradeableLock">
        /// <c>true</c> to obtain an upgradeable lock; otherwise <c>false</c>.
        /// </param>
        public LockReader(ReaderWriterLockSlim lockReference, int timeout, bool obtainUpgradeableLock)
        {
            Guard.AssertNotNull(lockReference, "lockReference");

            _upgradableLockRequested = obtainUpgradeableLock;
            _lockReferenceSlim = lockReference;

            // NOTE: This will fail if there is an existing reader lock for this thread and the recursion policy doesn't allow nested read locks
            if (_upgradableLockRequested)
            {
                if (_lockReferenceSlim.TryEnterUpgradeableReadLock(timeout) == false)
                {
                    throw new TimeoutException();
                }
            }
            else
            {
                if (_lockReferenceSlim.TryEnterReadLock(timeout) == false)
                {
                    throw new TimeoutException();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockReader"/> class using the provided lock reference.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        public LockReader(ReaderWriterLock lockReference)
            : this(lockReference, Timeout.Infinite)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockReader"/> class using the provided lock reference and timeout value.
        /// </summary>
        /// <param name="lockReference">
        /// The lock reference.
        /// </param>
        /// <param name="timeout">
        /// The timeout value within which the lock must be entered, measured in milliseconds.
        /// </param>
        public LockReader(ReaderWriterLock lockReference, int timeout)
        {
            Guard.AssertNotNull(lockReference, "lockReference");

            _lockReference = lockReference;

            // Check if there is a writer lock already for this thread
            if (lockReference.IsWriterLockHeld == false)
            {
                try
                {
                    // Only cause a reader lock if the current thread doesn't already have a writer lock
                    lockReference.AcquireReaderLock(timeout);
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
        /// </remarks>
        public void Dispose()
        {
            // Check if this instance is already disposed
            if (Disposed)
            {
                return;
            }

            Disposed = true;

            // Check if there is a slim lock
            if (_lockReferenceSlim != null)
            {
                // There is a ReaderWriterLockSlim
                if (_upgradableLockRequested)
                {
                    if (_lockReferenceSlim.IsUpgradeableReadLockHeld)
                    {
                        _lockReferenceSlim.ExitUpgradeableReadLock();
                    }
                }
                else
                {
                    // Check if there is a read lock
                    if (_lockReferenceSlim.IsReadLockHeld)
                    {
                        // Exit the read lock
                        _lockReferenceSlim.ExitReadLock();
                    }
                }
            }
            else
            {
                // There is a ReaderWriterLock
                // If there is a writer lock, we have not called for a reader lock in this instance
                if (_lockReference.IsWriterLockHeld)
                {
                    return;
                }

                // Check if there is still a read lock
                if (_lockReference.IsReaderLockHeld)
                {
                    // Release the read lock
                    _lockReference.ReleaseReaderLock();
                }
            }
        }
    }
}