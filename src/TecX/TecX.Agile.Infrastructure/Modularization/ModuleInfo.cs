namespace TecX.Agile.Infrastructure.Modularization
{
    using System;

    using TecX.Common;

    public class ModuleInfo
    {
        public ModuleInfo()
        {
        }

        public ModuleInfo(Type moduleType)
        {
            Guard.AssertNotNull(moduleType, "moduleType");

            Name = moduleType.Name;
            ModuleType = moduleType.AssemblyQualifiedName;
        }

        public string Name { get; set; }

        public string ModuleType { get; set; }
    }
}