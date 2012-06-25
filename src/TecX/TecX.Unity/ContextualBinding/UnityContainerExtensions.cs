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
            Type @from, 
            Type to, 
            LifetimeManager lifetime, 
            Predicate<IRequest> predicate, 
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(predicate, "predicate");

            var configuration = container.Configure<IContextualBinding>();

            if (configuration == null)
            {
                ContextualBindingExtension extension = new ContextualBindingExtension();

                container.AddExtension(extension);

                configuration = extension;
            }

            configuration.RegisterType(@from, to, lifetime, predicate, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(
            this IUnityContainer container, 
            LifetimeManager lifetime, 
            Predicate<IRequest> predicate, 
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), lifetime, predicate, injectionMembers);
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(
            this IUnityContainer container, 
            Predicate<IRequest> predicate, 
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, typeof(TFrom), typeof(TTo), new TransientLifetimeManager(), predicate, injectionMembers);
        }

        public static IUnityContainer RegisterInstance<TFrom>(
            this IUnityContainer container, 
            object instance, 
            Predicate<IRequest> predicate)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterInstance<TFrom>(container, instance, new ContainerControlledLifetimeManager(), predicate);
        }

        public static IUnityContainer RegisterInstance<TFrom>(
            this IUnityContainer container, 
            object instance, 
            LifetimeManager lifetime, 
            Predicate<IRequest> predicate)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterInstance(container, typeof(TFrom), instance, lifetime, predicate);
        }

        public static IUnityContainer RegisterInstance(
            this IUnityContainer container, 
            Type @from, 
            object instance, 
            LifetimeManager lifetime, 
            Predicate<IRequest> predicate)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(predicate, "predicate");
            Guard.AssertNotNull(instance, "instance");

            var configuration = container.Configure<IContextualBinding>();

            if (configuration == null)
            {
                ContextualBindingExtension extension = new ContextualBindingExtension();

                container.AddExtension(extension);

                configuration = extension;
            }

            configuration.RegisterInstance(@from, instance, lifetime, predicate);

            return container;
        }
    }
}