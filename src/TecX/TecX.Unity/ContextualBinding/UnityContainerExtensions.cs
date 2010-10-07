using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterType<TTo, TFrom>(this IUnityContainer container,
                                                               Func<IRequest, bool> shouldResolveTo)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(shouldResolveTo, "shouldResolveTo");

            var configuration = container.Configure<IContextualBindingConfiguration>();

            if (configuration == null)
                throw new InvalidOperationException("ContextualBindingExtension must be registered with the container!");

            configuration.Register<TTo, TFrom>(shouldResolveTo);

            return container;
        }
    }
}