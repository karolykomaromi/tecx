namespace Hydra.Import
{
    using System.Collections.Generic;

    public interface IDataReader<out T> : IEnumerable<T>
    {
        ImportMessages Messages { get; }
    }
}