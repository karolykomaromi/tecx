namespace Hydra.Import
{
    using System.Collections;
    using System.Collections.Generic;

    public class NullImportReader<T> : IImportReader<T>
    {
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