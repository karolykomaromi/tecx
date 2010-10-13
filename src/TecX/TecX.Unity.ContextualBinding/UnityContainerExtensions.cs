using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer container,
                                                               Func<IRequest, bool> shouldResolve)
        {
            return RegisterType<TFrom, TTo>(container, shouldResolve, null);
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(this IUnityContainer container, 
            Func<IRequest, bool> shouldResolve, LifetimeManager lifetimeManager)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(shouldResolve, "shouldResolve");

            var configuration = container.Configure<IContextualBindingConfiguration>();

            if (configuration == null)
                throw new InvalidOperationException("ContextualBindingExtension must be registered with the container!");

            configuration.Register<TFrom, TTo>(shouldResolve, lifetimeManager);

            return container;
        }
    }
}