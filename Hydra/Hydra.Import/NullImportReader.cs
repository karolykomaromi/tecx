namespace Hydra.Import
{
    using System.Collections;
    using System.Collections.Generic;

    public class NullImportReader<T> : IImportReader<T>
    {
        public ImportMessages Messages
        {
            get { return new ImportMessages(); }
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}