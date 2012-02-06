namespace TecX.Unity.TypedFactory
{
    using System;

    using Microsoft.Practices.Unity;

    public interface ITypedFactoryConfiguration : IUnityContainerExtensionConfigurator
    {
        void RegisterFactory(Type factoryType, ITypedFactoryComponentSelector selector);
    }
}