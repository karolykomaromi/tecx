namespace TecX.CaliburnEx.Modularization
{
    using System.Collections.Generic;

    public interface IModuleCatalog
    {
        IEnumerable<ModuleInfo> Modules { get; }

        void AddModule(ModuleInfo module);
    }
}