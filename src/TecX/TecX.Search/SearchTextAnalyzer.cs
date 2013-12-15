namespace TecX.Search
{
    using TecX.Common;
    using TecX.Search.Parse;
    using TecX.Search.Split;

    public class SearchTextAnalyzer
    {
        private readonly StringSplitStrategyChain splitChain;

        private readonly ParseStrategyChain parseChain;

        public SearchTextAnalyzer()
        {
            this.splitChain = new StringSplitStrategyChain();

            this.splitChain.Initialize();

            this.parseChain = new ParseStrategyChain();

            this.parseChain.Initialize();
        }

        public StringSplitParameterCollection Split(string searchTermsToAnalyze)
        {
            Guard.AssertNotEmpty(searchTermsToAnalyze, "searchTermsToAnalyze");

            this.splitChain.Split(searchTermsToAnalyze);

            return this.splitChain.Result.SplitResults;
        }

        public SearchParameterCollection Parse(StringSplitParameterCollection rawSearchTerms)
        {
            Guard.AssertNotNull(rawSearchTerms, "rawSearchTerms");

            SearchParameterCollection searchParameterCollection = new SearchParameterCollection();

            foreach (var rawSearchTerm in rawSearchTerms)
            {
                var parameter = this.parseChain.Parse(rawSearchTerm);

                if (parameter != SearchParameter.Empty)
                {
                    searchParameterCollection.Add(parameter);
                }
            }

            return searchParameterCollection;
        }

        public SearchParameterCollection Process(string searchTermsToProcess)
        {
            Guard.AssertNotEmpty(searchTermsToProcess, "searchTermsToProcess");

            StringSplitParameterCollection snippets = this.Split(searchTermsToProcess);

            SearchParameterCollection parameters = this.Parse(snippets);

            return parameters;
        }
    }
}
