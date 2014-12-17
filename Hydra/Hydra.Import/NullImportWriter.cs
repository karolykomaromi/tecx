namespace Hydra.Import
{
    using System.Collections.Generic;

    public class NullImportWriter<T> : IImportWriter<T>
    {
        public ImportResult Write(IEnumerable<T> items)
        {
            return new ImportFailed();
        }
    }
}