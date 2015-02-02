using System.Collections.ObjectModel;

namespace TecX.Agile.Remote.WebSockets
{
    /// <summary>
    /// Collection used to manage active service sessions.
    /// </summary>
    /// <typeparam name="TService">Type of service</typeparam>
    public class ServiceCollection<TService> : KeyedCollection<int, TService> where TService : class
    {
        /// <summary>
        /// Get the key for this entry.
        /// </summary>
        /// <param name="item">Entry to get key for.</param>
        /// <returns>The key value.</returns>
        protected override int GetKeyForItem(TService item)
        {
            return item.GetHashCode();
        }
    }
}