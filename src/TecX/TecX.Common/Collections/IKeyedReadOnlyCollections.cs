namespace TecX.Common.Collections
{
    using System.Collections.Generic;

    public interface IKeyedReadOnlyCollections<in TKey, TValue> : IEnumerable<TValue>
    {
        bool TryGetValue(TKey key, out TValue value);
    }
}