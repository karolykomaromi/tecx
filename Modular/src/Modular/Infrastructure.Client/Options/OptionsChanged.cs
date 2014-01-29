namespace Infrastructure.Options
{
    using System.Diagnostics.Contracts;

    public class OptionsChanged
    {
        private readonly IOptions options;
        private readonly Option optionName;

        public OptionsChanged(IOptions options, Option optionName)
        {
            Contract.Requires(options != null);

            this.options = options;
            this.optionName = optionName;
        }

        public IOptions Options
        {
            get { return this.options; }
        }

        public Option OptionName
        {
            get { return this.optionName; }
        }
    }
}