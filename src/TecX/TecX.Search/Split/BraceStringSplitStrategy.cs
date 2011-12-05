namespace TecX.Search.Split
{
    using System;

    using TecX.Common;
    using TecX.Search;

    public class BraceStringSplitStrategy : StringSplitStrategy
    {
        public override void Split(StringSplitContext context)
        {
            Guard.AssertNotNull(context, "context");

            string[] snippets = context.StringToSplit.Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries);

            StringCrawler crawler = new StringCrawler("(", ")");

            crawler.Crawl(snippets);

            foreach (StringSplitParameter parameter in crawler.Sequences)
            {
                context.SplitResults.Add(parameter);

                var composite = parameter as CompositeStringSplitParameter;

                if (composite != null)
                {
                    foreach (StringSplitParameter splitParameter in composite)
                    {
                        context.StringToSplit = context.StringToSplit.Replace(splitParameter.Value, string.Empty);
                    }
                }
            }

            context.StringToSplit = Constants.RegularExpressions.Braces.Replace(context.StringToSplit, string.Empty);
        }
    }
}