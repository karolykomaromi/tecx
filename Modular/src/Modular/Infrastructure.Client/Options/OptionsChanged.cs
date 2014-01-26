namespace Infrastructure.Options
{
    using System.Diagnostics.Contracts;

    public class OptionsChanged
    {
        private readonly IOptions options;

        public OptionsChanged(IOptions options)
        {
            Contract.Requires(options != null);

            this.options = options;
        }

        public IOptions Options
        {
            get
            {
                return this.options;
            }
        }
    }
}