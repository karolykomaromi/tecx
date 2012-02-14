namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public interface IContextualBinding : IUnityContainerExtensionConfigurator
    {
        object this[string key] { get; set; }

        void RegisterType(Type @from, Type to, LifetimeManager lifetime, Predicate<IBindingContext, IBuilderContext> predicate, params InjectionMember[] injectionMembers);

        void RegisterInstance(Type @from, object instance, LifetimeManager lifetime, Predicate<IBindingContext, IBuilderContext> predicate);
    }
}