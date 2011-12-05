namespace TecX.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using TecX.Common;
    using TecX.Search.Split;

    public class FullTextSearchTermProcessor
    {
        private readonly StringSplitStrategyChain chain;

        public FullTextSearchTermProcessor()
        {
            this.chain = new StringSplitStrategyChain();

            this.Initialize();
        }

        protected StringSplitStrategyChain Chain
        {
            get
            {
                return this.chain;
            }
        }

        public virtual IEnumerable<SearchTerm> Analyze(IEnumerable<Message> unprocessedMessages)
        {
            Guard.AssertNotNull(unprocessedMessages, "unprocessedMessages");

            List<SearchTerm> searchTerms = new List<SearchTerm>(100);

            foreach (Message msg in unprocessedMessages)
            {
                string fullText = this.GetFullText(msg);

                this.Chain.Split(fullText);

                var terms = this.Chain.Result.SplitResults
                    .Where(ssr => ssr.Value.Length > 1)
                    .Select(ssr => new SearchTerm { MessageId = msg.Id, Text = ssr.Value.ToUpperInvariant() });

                searchTerms.AddRange(terms);
            }

            return searchTerms;
        }

        protected virtual void Initialize()
        {
            this.Chain.AddStrategy(new AlphabeticalStringSplitStrategy());
            this.Chain.AddStrategy(new NumericalStringSplitStrategy());
        }

        protected virtual string GetFullText(Message message)
        {
            Guard.AssertNotNull(message, "message");

            StringBuilder sb = new StringBuilder(200);

            sb.Append(message.Source);
            sb.Append(Constants.Blank);
            sb.Append(message.MessageText);

            return sb.ToString();
        }
    }
}