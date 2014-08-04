namespace TecX.Common.Pipes
{
    using System.Collections.Generic;

    public static class PipelineExtensions
    {
        public static void Start<TIn, TOut>(this Filter<TIn, TOut> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            var enumerable = filter.Process(null);

            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext()) { }
        }

        public static Filter<TIn, TOut> Pipe<TIn, TOut>(this IEnumerable<TIn> enumerable, Filter<TIn, TOut> filter)
        {
            Guard.AssertNotNull(enumerable, "enumerable");
            Guard.AssertNotNull(filter, "filter");

            return new PipelineStartDummy<TIn, TOut>(enumerable, filter);
        }

        private class PipelineStartDummy<TIn, TOut> : Filter<TIn, TOut>
        {
            private readonly IEnumerable<TIn> enumerable;

            private readonly Filter<TIn, TOut> filter;

            public PipelineStartDummy(IEnumerable<TIn> enumerable, Filter<TIn, TOut> filter)
            {
                Guard.AssertNotNull(enumerable, "enumerable");
                Guard.AssertNotNull(filter, "filter");

                this.enumerable = enumerable;
                this.filter = filter;
            }

            public override IEnumerable<TOut> Process(IEnumerable<TIn> input)
            {
                return this.filter.Process(this.enumerable);
            }
        }
    }
}