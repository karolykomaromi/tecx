namespace Hydra.Import
{
    using System.Collections.Generic;

    public class NullDataWriter<T> : IDataWriter<T>
    {
        public ImportResult Write(IEnumerable<T> items)
        {
            return new ImportFailed();
        }
    }
}