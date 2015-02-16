namespace Hydra.Import.Data
{
    using System.Collections.Generic;
    using Hydra.Import.Results;

    public class NullDataWriter<T> : IDataWriter<T>
    {
        public ImportResult Write(IEnumerable<T> items)
        {
            return new ImportFailed();
        }
    }
}