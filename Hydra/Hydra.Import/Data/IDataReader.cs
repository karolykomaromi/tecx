namespace Hydra.Import.Data
{
    using System.Collections.Generic;
    using Hydra.Import.Messages;

    public interface IDataReader<out T> : IEnumerable<T>
    {
        ImportMessages Messages { get; }
    }
}