namespace Hydra.Import.Data
{
    using System.Collections.Generic;
    using Hydra.Import.Results;

    public interface IDataWriter<in T>
    {
        ImportResult Write(IEnumerable<T> items);
    }
}