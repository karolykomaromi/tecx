namespace TecX.CaliburnEx.Modularization
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

            this.Name = moduleType.Name;
            this.ModuleType = moduleType.AssemblyQualifiedName;
        }

        public string Name { get; set; }

        public string ModuleType { get; set; }
    }
}