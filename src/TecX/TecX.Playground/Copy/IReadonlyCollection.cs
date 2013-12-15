using System.Collections.Generic;

namespace TecX.Playground.Copy
{
    public interface IReadonlyCollection<out T> : IEnumerable<T>
    {
        int Count { get; }
    }
}