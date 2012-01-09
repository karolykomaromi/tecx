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
            Predicate<IBindingContext, IBuilderContext> isMatch, 
            LifetimeManager lifetime, 
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(isMatch, "isMatch");

            var configuration = container.Configure<IContextualBindingConfigurator>();

            if (configuration == null)
            {
                throw new ContextualBindingException("ContextualBindingExtension must be registered with the container!");
            }

            configuration.RegisterType(from, to, isMatch, lifetime, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(
            this IUnityContainer container,
            Predicate<IBindingContext, IBuilderContext> isMatch,
            LifetimeManager lifetime, 
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), isMatch, lifetime, injectionMembers);
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(
            this IUnityContainer container,
            Predicate<IBindingContext, IBuilderContext> isMatch,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), isMatch, new TransientLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterInstance<TFrom>(
            this IUnityContainer container, 
            object instance, 
            Predicate<IBindingContext, IBuilderContext> isMatch)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterInstance<TFrom>(container, instance, isMatch, new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer RegisterInstance<TFrom>(
            this IUnityContainer container, 
            object instance,
            Predicate<IBindingContext, IBuilderContext> isMatch, 
            LifetimeManager lifetime)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotNull(instance, "instance");

            var configuration = container.Configure<IContextualBindingConfigurator>();

            if (configuration == null)
            {
                throw new ContextualBindingException("ContextualBindingExtension must be registered with the container!");
            }

            configuration.RegisterInstance(typeof(TFrom), instance, isMatch, lifetime);

            return container;
        }
    }
}