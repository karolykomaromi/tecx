namespace TecX.Agile.Infrastructure.Modularization
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ModuleManager : IModuleManager
    {
        private readonly IModuleCatalog _catalog;

        private readonly IUnityContainer _container;

        public ModuleManager(IModuleCatalog catalog, IUnityContainer container)
        {
            Guard.AssertNotNull(catalog, "catalog");
            Guard.AssertNotNull(container, "container");

            _catalog = catalog;
            _container = container;
        }

        public void Run()
        {
            foreach (var moduleInfo in _catalog.Modules)
            {
                Type moduleType = Type.GetType(moduleInfo.ModuleType);

                IModule module = (IModule)_container.Resolve(moduleType, moduleInfo.Name);

                module.Initialize();
            }
        }
    }
}