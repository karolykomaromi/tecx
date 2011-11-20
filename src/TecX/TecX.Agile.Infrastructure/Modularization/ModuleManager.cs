namespace TecX.Agile.Infrastructure.Modularization
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ModuleManager : IModuleManager
    {
        private readonly IModuleCatalog catalog;

        private readonly IUnityContainer container;

        public ModuleManager(IModuleCatalog catalog, IUnityContainer container)
        {
            Guard.AssertNotNull(catalog, "catalog");
            Guard.AssertNotNull(container, "container");

            this.catalog = catalog;
            this.container = container;
        }

        public void Run()
        {
            foreach (var moduleInfo in this.catalog.Modules)
            {
                Type moduleType = Type.GetType(moduleInfo.ModuleType);

                IModule module = (IModule)this.container.Resolve(moduleType, moduleInfo.Name);

                module.Initialize();
            }
        }
    }
}