namespace TecX.Search.Data.Test.TestObjects
{
    using System.Collections.Generic;

    using TecX.Search;

    public class NullFullTextSearchTermProcessor : FullTextSearchTermProcessor
    {
        public override IEnumerable<SearchTerm> Analyze(IEnumerable<Message> unprocessedMessages)
        {
            return new SearchTerm[0];
        }
    }
}