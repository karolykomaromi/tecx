namespace TecX.Common.Test.Pipes
{
    using System.Collections;
    using System.Collections.Generic;

    public class Numbers : IEnumerable<int>
    {
        private readonly int max;

        public Numbers(int max)
        {
            this.max = max;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 1; i <= this.max; i++)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}