namespace TecX.Search.Parse
{
    using System.Collections.Generic;
    using System.Linq;
    
    using TecX.Common;
    using TecX.Search.Split;

    public class ParseStrategyChain
    {
        private readonly List<ParameterParseStrategy> strategies;

        public ParseStrategyChain()
        {
            this.strategies = new List<ParameterParseStrategy>();
        }

        public IEnumerable<ParameterParseStrategy> Strategies
        {
            get { return this.strategies; }
        }

        public void AddStrategy<TStrategy>(TStrategy strategy)
            where TStrategy : ParameterParseStrategy
        {
            Guard.AssertNotNull(strategy, "strategy");

            if (this.strategies.OfType<TStrategy>().Count() == 0)
            {
                this.strategies.Add(strategy);
            }
        }

        public SearchParameter Parse(StringSplitParameter rawSearchTerm)
        {
            Guard.AssertNotNull(rawSearchTerm, "rawSearchTerm");

            var context = new ParameterParseContext(rawSearchTerm);

            foreach (var strategy in this.Strategies)
            {
                strategy.Parse(context);

                if (context.ParseComplete)
                {
                    return context.Parameter;
                }
            }

            return SearchParameter.Empty;
        }
    }
}