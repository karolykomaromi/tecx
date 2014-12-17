namespace Hydra.Import
{
    using System.Collections.Generic;

    public interface IImportReader<out T> : IEnumerable<T>
    {
    }
}