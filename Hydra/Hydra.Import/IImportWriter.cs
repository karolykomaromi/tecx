namespace Hydra.Import
{
    using System.Collections.Generic;

    public interface IImportWriter<in T>
    {
        ImportResult Write(IEnumerable<T> items);
    }
}