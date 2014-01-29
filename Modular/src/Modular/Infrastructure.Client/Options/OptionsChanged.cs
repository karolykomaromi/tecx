namespace Infrastructure.Options
{
    using System.Diagnostics.Contracts;

    public class OptionsChanged
    {
        private readonly IOptions options;
        private readonly OptionName optionName;

        public OptionsChanged(IOptions options, OptionName optionName)
        {
            Contract.Requires(options != null);

            this.options = options;
            this.optionName = optionName;
        }

        public IOptions Options
        {
            get { return this.options; }
        }

        public OptionName OptionName
        {
            get { return this.optionName; }
        }
    }
}