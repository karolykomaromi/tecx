namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using Infrastructure.Options;

    public class OptionsInitializer : IModuleInitializer
    {
        private readonly CompositeOptions options;

        public OptionsInitializer(CompositeOptions options)
        {
            Contract.Requires(options != null);

            this.options = options;
        }

        public void Initialize(UnityModule module)
        {
            IOptions o = module.CreateModuleOptions();

            if ((o as NullOptions) == null)
            {
                this.options.Add(o);
            }
        }
    }
}
