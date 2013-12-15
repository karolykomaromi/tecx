namespace TecX.Search.Parse
{
    using TecX.Common;
    using TecX.Search.Split;

    public class OrParseStrategy : ParameterParseStrategy
    {
        private readonly ParseStrategyChain chain;

        public OrParseStrategy(ParseStrategyChain chain)
        {
            Guard.AssertNotNull(chain, "chain");

            this.chain = chain;
        }

        protected override void ParseCore(ParameterParseContext context)
        {
            OrStringSplitParameter or = context.StringToParse as OrStringSplitParameter;

            OrSearchParameter composite = new OrSearchParameter();

            if (or != null)
            {
                foreach (StringSplitParameter parameter in or)
                {
                    SearchParameter searchParameter = this.chain.Parse(parameter);

                    composite.Add(searchParameter);
                }
            }
        }
    }
}
