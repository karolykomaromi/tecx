namespace TecX.Unity.Proxies
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterLazyProxy(this IUnityContainer container, Action<LazyProxyConfiguration> action)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(action, "action");

            IProxyGenerator generator = container.Configure<IProxyGenerator>();

            if (generator == null)
            {
                var extension = new ProxyGeneratorExtension();
                generator = extension;
                container.AddExtension(extension);
            }

            LazyProxyConfiguration configuration = new LazyProxyConfiguration();

            action(configuration);

            configuration.Validate();

            container.RegisterType(
                configuration.Contract,
                configuration.ServiceImplementation,
                configuration.ServiceUniqueRegistrationName,
                configuration.ServiceLifetime,
                configuration.ServiceInjectionMembers);

            Type lazyProxyType = generator.CreateLazyInstantiationProxy(configuration.Contract);

            container.RegisterType(
                configuration.Contract,
                lazyProxyType,
                configuration.ProxyUniqueRegistrationName,
                configuration.ProxyLifetime,
                configuration.ProxyInjectionMembers);

            return container;
        }
    }
}
