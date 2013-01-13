namespace TecX.Search.Split
{
    using System;
    using System.Text.RegularExpressions;

    using TecX.Common;
    using TecX.Search;

    public class OrStringSplitStrategy : StringSplitStrategy
    {
        public override void Split(StringSplitContext context)
        {
            Guard.AssertNotNull(context, "context");

            var matches = Constants.RegularExpressions.OrClause.Matches(context.StringToSplit);

            foreach (Match match in matches)
            {
                var or = new OrStringSplitParameter();

                string[] snippets = match.Value.Replace(@"|", Constants.Blank).Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string snippet in snippets)
                {
                    or.Add(new StringSplitParameter(snippet));
                }

                context.SplitResults.Add(or);
                context.StringToSplit = context.StringToSplit.Replace(match.Value, string.Empty).Trim();
            }
        }
    }
}