namespace TecX.Unity.ContextualBinding
{
    using TecX.Common;

    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public interface IContextualBindingConfigurator : IUnityContainerExtensionConfigurator
    {
        object this[string key] { get; set; }

        void RegisterType(
            Type from,
            Type to,
            Predicate<IBindingContext, IBuilderContext> isMatch,
            LifetimeManager lifetime,
            params InjectionMember[] injectionMembers);

        void RegisterInstance(Type from, object instance, Predicate<IBindingContext, IBuilderContext> matches, LifetimeManager lifetime);
    }
}