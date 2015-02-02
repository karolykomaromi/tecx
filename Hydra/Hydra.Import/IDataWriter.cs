namespace Hydra.Import
{
    using System.Collections.Generic;

    public interface IDataWriter<in T>
    {
        ImportResult Write(IEnumerable<T> items);
    }
}