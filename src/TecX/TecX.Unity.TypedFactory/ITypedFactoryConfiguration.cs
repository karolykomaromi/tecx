using Microsoft.Practices.Unity;

namespace TecX.Unity.TypedFactory
{
    public interface ITypedFactoryConfiguration : IUnityContainerExtensionConfigurator
    {
        void RegisterFactory<TFactory>() where TFactory : class;

        void RegisterFactory<TFactory>(ITypedFactoryComponentSelector selector) where TFactory : class;
    }
}