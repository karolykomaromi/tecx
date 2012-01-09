namespace TecX.CaliburnEx.Modularization
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public class ModuleCatalog : IModuleCatalog
    {
        private readonly List<ModuleInfo> modules;

        public ModuleCatalog()
        {
            this.modules = new List<ModuleInfo>();
        }

        public IEnumerable<ModuleInfo> Modules
        {
            get
            {
                return this.modules;
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

            this.AddModule(module);
        }

        public void AddModule(ModuleInfo module)
        {
            Guard.AssertNotNull(module, "module");

            this.modules.Add(module);
        }
    }
}