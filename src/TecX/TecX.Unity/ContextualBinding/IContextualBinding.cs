namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.Unity;
    
    public interface IContextualBinding : IUnityContainerExtensionConfigurator
    {
        object this[string key] { get; set; }

        void RegisterType(Type @from, Type to, LifetimeManager lifetime, Predicate<IRequest> predicate, params InjectionMember[] injectionMembers);

        void RegisterInstance(Type @from, object instance, LifetimeManager lifetime, Predicate<IRequest> predicate);

        bool Remove(string key);
    }
}