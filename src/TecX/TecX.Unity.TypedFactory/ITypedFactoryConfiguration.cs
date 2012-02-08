namespace TecX.Unity.TypedFactory
{
    using System;

    using Microsoft.Practices.Unity;

    public interface ITypedFactoryConfiguration : IUnityContainerExtensionConfigurator
    {
        void RegisterFactory(Type factoryType, string name, ITypedFactoryComponentSelector selector, LifetimeManager lifetime, params InjectionMember[] injectionMembers);
    }
}