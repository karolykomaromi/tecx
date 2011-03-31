using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterType(this IUnityContainer container, Type from, Type to,
            Predicate<IBindingContext, IBuilderContext> matches, LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(matches, "matches");

            var configuration = container.Configure<IContextualBindingConfiguration>();

            if (configuration == null)
                throw new ContextualBindingException("ContextualBindingExtension must be registered with the container!");

            configuration.RegisterType(from, to, matches, lifetime, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer container, Predicate<IBindingContext, IBuilderContext> matches,
            LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), matches, lifetime, injectionMembers);
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer container, Predicate<IBindingContext, IBuilderContext> matches,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), matches, new TransientLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterInstance<TFrom>(this IUnityContainer container, object instance,
            Predicate<IBindingContext, IBuilderContext> matches)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterInstance<TFrom>(container, instance, matches, new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer RegisterInstance<TFrom>(this IUnityContainer container, object instance,
            Predicate<IBindingContext, IBuilderContext> matches, LifetimeManager lifetime)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(matches, "matches");
            Guard.AssertNotNull(instance, "instance");

            var configuration = container.Configure<IContextualBindingConfiguration>();

            if (configuration == null)
                throw new ContextualBindingException("ContextualBindingExtension must be registered with the container!");

            configuration.RegisterInstance(typeof(TFrom), instance, matches, lifetime);

            return container;
        }
    }
}