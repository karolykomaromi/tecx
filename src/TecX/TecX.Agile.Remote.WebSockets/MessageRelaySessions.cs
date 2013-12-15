using System;
using System.Collections.Generic;
using System.Threading;

namespace TecX.Agile.Remote.WebSockets
{
    public class MessageRelaySessions : IDisposable
    {
        private readonly ServiceCollection<MessageRelayService> _innerCache = new ServiceCollection<MessageRelayService>();
        private readonly ReaderWriterLockSlim _thisLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Relaying the contents from one session to all other sessions.
        /// </summary>
        /// <param name="current">The current session relaying the content.</param>
        /// <param name="value">Content to relay.</param>
        public void RelayMessage(MessageRelayService current, string value)
        {
            List<MessageRelayService> defunct = null;
            _thisLock.EnterReadLock();
            try
            {
                foreach (var entry in _innerCache)
                {
                    if (current == entry)
                        continue;

                    try
                    {
                        entry.SendMessage(value);
                    }
                    catch
                    {
                        if (defunct == null)
                        {
                            defunct = new List<MessageRelayService>();
                        }

                        defunct.Add(entry);
                    }
                }
            }
            finally
            {
                _thisLock.ExitReadLock();
            }

            if (defunct != null)
            {
                _thisLock.EnterWriteLock();
                try
                {
                    foreach (var entry in defunct)
                    {
                        _innerCache.Remove(entry);
                    }
                }
                finally
                {
                    _thisLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Attempting to add another session to the collection.
        /// </summary>
        /// <param name="entry">Session to add.</param>
        /// <returns>true if session was added; false otherwise.</returns>
        public bool TryAdd(MessageRelayService entry)
        {
            _thisLock.EnterUpgradeableReadLock();
            try
            {
                if (_innerCache.Contains(entry))
                {
                    return false;
                }

                _thisLock.EnterWriteLock();
                try
                {
                    _innerCache.Add(entry);
                    return true;
                }
                finally
                {
                    _thisLock.ExitWriteLock();
                }
            }
            finally
            {
                _thisLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Attempting to remove a session from the collection.
        /// </summary>
        /// <param name="entry">Session to remove.</param>
        public void Remove(MessageRelayService entry)
        {
            ThreadPool.QueueUserWorkItem(RemoveInternal, entry);
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        public void Dispose()
        {
            _thisLock.Dispose();
        }

        private void RemoveInternal(object state)
        {
            var entry = state as MessageRelayService;
            if (entry != null)
            {
                _thisLock.EnterWriteLock();
                try
                {
                    _innerCache.Remove(entry);
                }
                finally
                {
                    _thisLock.ExitWriteLock();
                }
            }
        }
    }
}
