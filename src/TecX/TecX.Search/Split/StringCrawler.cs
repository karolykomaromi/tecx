namespace TecX.Search.Split
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public enum CrawlState
    {
        Closed,

        Open,
    }

    public class StringCrawler
    {
        private readonly string sequenceStartChar;

        private readonly string sequenceTerminatingChar;

        private readonly StringSplitParameterCollection sequences;

        private CrawlState state;

        public StringCrawler(string sequenceStartChar, string sequenceTerminatingChar)
        {
            Guard.AssertNotEmpty(sequenceStartChar, "sequenceStartChar");
            Guard.AssertNotEmpty(sequenceTerminatingChar, "sequenceTerminatingChar");

            this.sequenceStartChar = sequenceStartChar;
            this.sequenceTerminatingChar = sequenceTerminatingChar;

            this.sequences = new StringSplitParameterCollection();
            this.state = CrawlState.Closed;
        }

        public StringSplitParameterCollection Sequences
        {
            get
            {
                return this.sequences;
            }
        }

        public void Crawl(IEnumerable<string> stringsToCrawl)
        {
            Guard.AssertNotNull(stringsToCrawl, "stringsToCrawl");

            this.Sequences.Clear();

            CompositeStringSplitParameter composite = new CompositeStringSplitParameter();
            this.Sequences.Add(composite);

            foreach (string snippet in stringsToCrawl)
            {
                if (this.state == CrawlState.Closed &&
                    snippet.StartsWith(this.sequenceStartChar, StringComparison.InvariantCultureIgnoreCase) &&
                    snippet.EndsWith(this.sequenceTerminatingChar, StringComparison.InvariantCultureIgnoreCase))
                {
                    string s = snippet
                                .Replace(this.sequenceStartChar, string.Empty)
                                .Replace(this.sequenceTerminatingChar, string.Empty)
                                .Trim();

                    composite.Add(new StringSplitParameter(s));
                    composite = new CompositeStringSplitParameter();
                    this.Sequences.Add(composite);
                    continue;
                }

                if (this.state == CrawlState.Closed &&
                    snippet.StartsWith(this.sequenceStartChar, StringComparison.InvariantCultureIgnoreCase))
                {
                    string s = snippet.Replace(this.sequenceStartChar, string.Empty).Trim();

                    if (!string.IsNullOrEmpty(s))
                    {
                        composite.Add(new StringSplitParameter(s));
                    }

                    this.state = CrawlState.Open;
                }

                if (this.state == CrawlState.Open &&
                    snippet.EndsWith(this.sequenceTerminatingChar, StringComparison.InvariantCultureIgnoreCase))
                {
                    string s = snippet.Replace(this.sequenceTerminatingChar, string.Empty).Trim();

                    if (!string.IsNullOrEmpty(s))
                    {
                        composite.Add(new StringSplitParameter(s));
                    }

                    this.state = CrawlState.Closed;

                    composite = new CompositeStringSplitParameter();
                    this.Sequences.Add(composite);
                }
            }
        }
    }
}