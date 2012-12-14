using System.Collections.Generic;

namespace TecX.TestTools.Data
{
    public interface IAppendOnlyCollection<T> : IEnumerable<T>
    {
        void Add(T item);
    }
}