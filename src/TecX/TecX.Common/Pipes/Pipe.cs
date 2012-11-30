namespace TecX.Common.Pipes
{
    using System.Collections.Generic;

    public class Pipe<TIn, T, TOut> : Filter<TIn, TOut>
    {
        private readonly Filter<TIn, T> source;

        private readonly Filter<T, TOut> destination;

        public Pipe(Filter<TIn, T> source, Filter<T, TOut> destination)
        {
            Guard.AssertNotNull(source, "source");
            Guard.AssertNotNull(destination, "destination");

            this.source = source;
            this.destination = destination;
        }

        public override sealed IEnumerable<TOut> Process(IEnumerable<TIn> input)
        {
            var x = this.source.Process(input);

            var result = this.destination.Process(x);

            return result;
        }
    }
}