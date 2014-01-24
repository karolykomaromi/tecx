namespace Infrastructure.Modularity
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class CompositeInitializer : IModuleInitializer
    {
        private readonly HashSet<IModuleInitializer> initializers;

        public CompositeInitializer(params IModuleInitializer[] initializers)
        {
            this.initializers = new HashSet<IModuleInitializer>(initializers ?? new IModuleInitializer[0]);
        }

        public void Initialize(UnityModule module)
        {
            foreach (IModuleInitializer initializer in this.initializers)
            {
                initializer.Initialize(module);
            }
        }

        public void Add(IModuleInitializer initializer)
        {
            Contract.Requires(initializer != null);

            this.initializers.Add(initializer);
        }
    }
}