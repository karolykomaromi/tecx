namespace TecX.TestTools.Data
{
    using System.Collections.Generic;

    public interface IAppendOnlyCollection<T> : IEnumerable<T>
    {
        void Add(T item);
    }
}