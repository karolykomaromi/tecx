using Microsoft.Practices.Unity;

namespace TecX.Unity.TypedFactory
{
    public interface ITypedFactoryConfiguration : IUnityContainerExtensionConfigurator
    {
        void RegisterFactory<TFactory>();
    }
}