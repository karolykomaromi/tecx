namespace TecX.Unity.TypedFactory
{
    using Microsoft.Practices.Unity;

    public interface ITypedFactoryConfiguration : IUnityContainerExtensionConfigurator
    {
        void RegisterFactory<TFactory>(ITypedFactoryComponentSelector selector) where TFactory : class;
    }
}