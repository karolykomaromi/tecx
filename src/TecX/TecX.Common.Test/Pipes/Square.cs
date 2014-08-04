namespace TecX.Common.Test.Pipes
{
    using System.Collections.Generic;

    using TecX.Common.Pipes;

    public class Square : Filter<int, int>
    {
        public override IEnumerable<int> Process(IEnumerable<int> input)
        {
            foreach (int i in input)
            {
                yield return i * i;
            }
        }
    }
}