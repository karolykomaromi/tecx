namespace Hydra.Import
{
    using System.Collections.Generic;

    public class NullDataReader<T> : DataReader<T>
    {
        public override IEnumerator<T> GetEnumerator()
        {
            yield break;
        }
    }
}