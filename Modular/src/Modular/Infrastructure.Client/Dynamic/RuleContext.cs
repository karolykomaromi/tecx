namespace Infrastructure.Dynamic
{
    using Infrastructure.Options;

    public class RuleContext : IRuleContext
    {
        private readonly IViewRegistry registry;

        private readonly IOptions options;

        public RuleContext(IViewRegistry registry, IOptions options)
        {
            this.registry = registry;
            this.options = options;
        }

        public IViewRegistry Registry
        {
            get { return this.registry; }
        }

        public IOptions Options
        {
            get { return this.options; }
        }
    }
}