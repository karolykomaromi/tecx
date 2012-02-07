namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterType(
            this IUnityContainer container,
            Type from,
            Type to,
            Predicate<IBindingContext, IBuilderContext> predicate,
            LifetimeManager lifetime,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(predicate, "predicate");

            var configuration = container.Configure<IContextualBindingConfigurator>();

            if (configuration == null)
            {
                throw new ContextualBindingException("ContextualBindingExtension must be registered with the container!");
            }

            configuration.RegisterType(from, to, predicate, lifetime, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(
            this IUnityContainer container,
            Predicate<IBindingContext, IBuilderContext> predicate,
            LifetimeManager lifetime,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), predicate, lifetime, injectionMembers);
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(
            this IUnityContainer container,
            Predicate<IBindingContext, IBuilderContext> predicate,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), predicate, new TransientLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterInstance<TFrom>(
            this IUnityContainer container,
            object instance,
            Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterInstance<TFrom>(container, instance, predicate, new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer RegisterInstance<TFrom>(
            this IUnityContainer container,
            object instance,
            Predicate<IBindingContext, IBuilderContext> predicate,
            LifetimeManager lifetime)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterInstance(container, typeof(TFrom), instance, predicate, lifetime);
        }

        public static IUnityContainer RegisterInstance(
            this IUnityContainer container,
            Type from,
            object instance,
            Predicate<IBindingContext, IBuilderContext> predicate,
            LifetimeManager lifetime)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(predicate, "predicate");
            Guard.AssertNotNull(instance, "instance");

            var configuration = container.Configure<IContextualBindingConfigurator>();

            if (configuration == null)
            {
                throw new ContextualBindingException("ContextualBindingExtension must be registered with the container!");
            }

            configuration.RegisterInstance(from, instance, predicate, lifetime);

            return container;
        }
    }
}