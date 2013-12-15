namespace TecX.Search.Split
{
    using System;
    using System.Linq;

    using TecX.Common;
    using TecX.Search;

    public class WhitespaceStringSplitStrategy : StringSplitStrategy
    {
        public override void Split(StringSplitContext context)
        {
            Guard.AssertNotNull(context, "context");

            var snippets = context.StringToSplit.Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries).ToList();

            snippets.ForEach(m =>
                {
                    context.SplitResults.Add(new StringSplitParameter(m));
                    context.StringToSplit = context.StringToSplit
                                                .Replace(m, string.Empty)
                                                .Trim();
                });
        }
    }
}
