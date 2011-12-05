namespace TecX.Search.Split
{
    using System.Linq;
    using System.Text.RegularExpressions;

    using TecX.Common;

    public abstract class RegexBasedStringSplitStrategy : StringSplitStrategy
    {
        private readonly Regex regex;

        protected RegexBasedStringSplitStrategy(Regex regex)
        {
            Guard.AssertNotNull(regex, "regex");

            this.regex = regex;
        }

        public override void Split(StringSplitContext context)
        {
            Guard.AssertNotNull(context, "context");

            var matches = this.regex.Matches(context.StringToSplit)
                .OfType<Match>()
                .Where(m => !string.IsNullOrEmpty(m.Value))
                .Select(m => m.Value)
                .ToList();

            matches.ForEach(m =>
                {
                    context.SplitResults.Add(new StringSplitParameter(m));
                    context.StringToSplit = context
                        .StringToSplit
                        .Replace(m, string.Empty)
                        .Trim();
                });
        }
    }
}