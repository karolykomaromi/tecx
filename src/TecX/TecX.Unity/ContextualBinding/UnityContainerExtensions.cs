namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Tracking;

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

            ContextualBinding context = container.Configure<ContextualBinding>();

            if (context == null)
            {
                context = new ContextualBinding();

                container.AddExtension(context);
            }

            context.RegisterType(@from, to, lifetime, predicate, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterType<TTo>(
            this IUnityContainer container,
            Predicate<IRequest> predicate,
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");

            return RegisterType(container, null, typeof(TTo), null, predicate, injectionMembers);
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

            ContextualBinding context = container.Configure<ContextualBinding>();

            if (context == null)
            {
                context = new ContextualBinding();

                container.AddExtension(context);
            }

            context.RegisterInstance(@from, instance, lifetime, predicate);

            return container;
        }
    }
}