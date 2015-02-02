namespace TecX.Search.Split
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class StringSplitStrategyChain
    {
        private readonly List<StringSplitStrategy> strategies;

        private StringSplitContext result;

        public StringSplitStrategyChain()
        {
            this.strategies = new List<StringSplitStrategy>();
        }

        public StringSplitContext Result
        {
            get
            {
                return this.result;
            }
        }

        public void AddStrategy<TStrategy>(TStrategy strategy)
            where TStrategy : StringSplitStrategy
        {
            Guard.AssertNotNull(strategy, "strategy");

            if (this.strategies.OfType<TStrategy>().Count() == 0)
            {
                this.strategies.Add(strategy);
            }
        }

        public void Split(string stringToSplit)
        {
            Guard.AssertNotEmpty(stringToSplit, "stringToSplit");

            var context = new StringSplitContext { StringToSplit = stringToSplit };

            foreach (var strategy in this.strategies)
            {
                strategy.Split(context);
            }

            this.result = context;
        }
    }
}