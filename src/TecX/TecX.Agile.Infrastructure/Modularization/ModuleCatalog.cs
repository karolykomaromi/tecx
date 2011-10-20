namespace TecX.Agile.Infrastructure.Modularization
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public class ModuleCatalog : IModuleCatalog
    {
        private readonly List<ModuleInfo> _modules;

        public ModuleCatalog()
        {
            _modules = new List<ModuleInfo>();
        }

        public IEnumerable<ModuleInfo> Modules
        {
            get
            {
                return _modules;
            }
        }

        public void AddModule(Type moduleType)
        {
            Guard.AssertNotNull(moduleType, "moduleType");

            ModuleInfo module = new ModuleInfo
                {
                    Name = moduleType.Name,
                    ModuleType = moduleType.AssemblyQualifiedName
                };

            AddModule(module);
        }

        public void AddModule(ModuleInfo module)
        {
            Guard.AssertNotNull(module, "module");

            _modules.Add(module);
        }
    }
}