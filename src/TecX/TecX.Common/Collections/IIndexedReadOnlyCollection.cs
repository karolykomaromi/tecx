namespace TecX.Common.Collections
{
    using System.Collections.Generic;

    public interface IIndexedReadOnlyCollection<T> : IEnumerable<T>
    {
        T this[int index] { get; }
    }
}