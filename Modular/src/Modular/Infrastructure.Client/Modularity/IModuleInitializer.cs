namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ModuleInitializerContract))]
    public interface IModuleInitializer
    {
        void Initialize(UnityModule module);
    }

    [ContractClassFor(typeof(IModuleInitializer))]
    internal abstract class ModuleInitializerContract : IModuleInitializer
    {
        public void Initialize(UnityModule module)
        {
            Contract.Requires(module != null);
        }
    }
}
