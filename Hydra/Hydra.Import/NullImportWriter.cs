namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;

    public class NullImportWriter<T> : IImportWriter<T>
    {
        public ImportResult Write(IEnumerable<T> items)
        {
            return new ImportFailed(new Exception());
        }
    }
}