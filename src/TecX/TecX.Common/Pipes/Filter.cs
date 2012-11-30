namespace TecX.Common.Pipes
{
    using System.Collections.Generic;

    public abstract class Filter<TIn, TOut>
    {
        public abstract IEnumerable<TOut> Process(IEnumerable<TIn> input);

        public Filter<TIn, TNext> Pipe<TNext>(Filter<TOut, TNext> next)
        {
            Guard.AssertNotNull(next, "next");

            return new Pipe<TIn, TOut, TNext>(this, next);
        }
    }
}