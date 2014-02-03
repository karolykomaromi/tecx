namespace Infrastructure.Options
{
    using System.Diagnostics.Contracts;

    public class OptionsChanged<TOptions> : IOptionsChanged<TOptions>
        where TOptions : IOptions
    {
        private readonly TOptions options;
        private readonly Option optionName;

        public OptionsChanged(TOptions options, Option optionName)
        {
            Contract.Requires(options != null);

            this.options = options;
            this.optionName = optionName;
        }

        public TOptions Options
        {
            get { return this.options; }
        }

        public Option OptionName
        {
            get { return this.optionName; }
        }
    }
}