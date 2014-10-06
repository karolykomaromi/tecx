namespace TecX.Search.Split
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using TecX.Common;
    using TecX.Search;

    public class DateTimeStringSplitStrategy : StringSplitStrategy
    {
        private readonly IFormatProvider formatProvider;

        public DateTimeStringSplitStrategy()
            : this(Defaults.Culture)
        {
        }

        public DateTimeStringSplitStrategy(IFormatProvider formatProvider)
        {
            Guard.AssertNotNull(formatProvider, "formatProvider");

            this.formatProvider = formatProvider;
        }

        public override void Split(StringSplitContext context)
        {
            Guard.AssertNotNull(context, "context");

            string[] snippets = context.StringToSplit.Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < snippets.Length - 1; i++)
            {
                var snippet1 = snippets[i];
                var snippet2 = snippets[i + 1];

                var snippet = snippet1 + Constants.Blank + snippet2;

                DateTime dt;
                if (DateTime.TryParse(snippet, this.formatProvider, DateTimeStyles.None, out dt))
                {
                    context.SplitResults.Add(new StringSplitParameter(snippet));

                    Regex regex = new Regex(snippet1 + @"\s+" + snippet2);
                    context.StringToSplit = regex.Replace(context.StringToSplit, string.Empty, 1).Trim();
                    i++;
                }
            }

            snippets = context.StringToSplit.Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < snippets.Length; i++)
            {
                var snippet = snippets[i];
                DateTime dt;
                if (DateTime.TryParse(snippet, this.formatProvider, DateTimeStyles.None, out dt))
                {
                    context.SplitResults.Add(new StringSplitParameter(snippet));
                    context.StringToSplit = context.StringToSplit.Replace(snippet, string.Empty).Trim();
                }
            }
        }
    }
}
