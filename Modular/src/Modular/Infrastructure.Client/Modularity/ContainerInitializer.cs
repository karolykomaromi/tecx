namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;

    public class ContainerInitializer : IModuleInitializer
    {
        private readonly IUnityContainer container;

        public ContainerInitializer(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public void Initialize(UnityModule module)
        {
            module.ConfigureContainer(this.container);
        }
    }
}