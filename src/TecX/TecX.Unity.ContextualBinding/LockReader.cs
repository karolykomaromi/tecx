using System;
using System.Threading;

namespace TecX.Unity.ContextualBinding
{
    /// <summary>
    /// The <see cref="LockWriter"/>
    ///   class is used to provide thread safe write access to a resource using a provided
    ///   <see cref="ReaderWriterLock"/> or <see cref="ReaderWriterLockSlim"/> instance.
    /// </summary>
    public sealed class LockWriter : IDisposable
    {
        private const string LockReferenceParameterName = "lockReference";

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
        public LockWriter(ReaderWriterLockSlim lockReference, int timeout)
        {
            // Checks whether the lockReference parameter has been supplied
            if (lockReference == null)
            {
                throw new ArgumentNullException(LockReferenceParameterName);
            }

            LockReferenceSlim = lockReference;

            // Acquire a write lock
            if (LockReferenceSlim.TryEnterWriteLock(timeout) == false)
            {
                throw new TimeoutException();
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

            if (LockReferenceSlim.IsWriteLockHeld)
            {
                LockReferenceSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LockWriter"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        private bool Disposed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lock reference slim.
        /// </summary>
        /// <value>
        /// The lock reference slim.
        /// </value>
        private ReaderWriterLockSlim LockReferenceSlim
        {
            get;
            set;
        }
    }

    /// <summary>
    /// The <see cref="LockReader"/>
    ///   class is used to provide thread safe read access to a resource using a provided 
    ///   <see cref="ReaderWriterLock"/> or <see cref="ReaderWriterLockSlim"/> instance.
    /// </summary>
    public sealed class LockReader : IDisposable
    {
        private const string LockReferenceParameterName = "lockReference";

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
            // Checks whether the lockReference parameter has been supplied
            if (lockReference == null)
            {
                throw new ArgumentNullException(LockReferenceParameterName);
            }

            UpgradableLockRequested = obtainUpgradeableLock;
            LockReferenceSlim = lockReference;

            // NOTE: This will fail if there is an existing reader lock for this thread and the recursion policy doesn't allow nested read locks
            if (UpgradableLockRequested)
            {
                if (LockReferenceSlim.TryEnterUpgradeableReadLock(timeout) == false)
                {
                    throw new TimeoutException();
                }
            }
            else
            {
                if (LockReferenceSlim.TryEnterReadLock(timeout) == false)
                {
                    throw new TimeoutException();
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
            // There is a ReaderWriterLockSlim
            if (UpgradableLockRequested)
            {
                if (LockReferenceSlim.IsUpgradeableReadLockHeld)
                {
                    LockReferenceSlim.ExitUpgradeableReadLock();
                }
            }
            else
            {
                // Check if there is a read lock
                if (LockReferenceSlim.IsReadLockHeld)
                {
                    // Exit the read lock
                    LockReferenceSlim.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LockReader"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        private bool Disposed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lock reference slim.
        /// </summary>
        /// <value>
        /// The lock reference slim.
        /// </value>
        private ReaderWriterLockSlim LockReferenceSlim
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether an upgradable lock was requested.
        /// </summary>
        /// <value>
        /// <c>true</c> if an upgradable lock was requested; otherwise, <c>false</c>.
        /// </value>
        private bool UpgradableLockRequested
        {
            get;
            set;
        }
    }
}
