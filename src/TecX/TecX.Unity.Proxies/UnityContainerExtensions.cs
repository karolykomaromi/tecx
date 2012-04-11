namespace TecX.Unity.Proxies
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterFaultTolerantProxy(this IUnityContainer container, Type contract, LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(contract, "contract");

            var proxyGenerator = container.Configure<IProxyGenerator>();

            if (proxyGenerator == null)
            {
                throw new InvalidOperationException("ProxyGeneratorExtension not registered with the container.");
            }

            var proxyType = proxyGenerator.CreateFaultTolerantProxy(contract);

            string uniqueName = Guid.NewGuid().ToString();

            container.RegisterType(null, contract, uniqueName, lifetime, injectionMembers);

            container.RegisterType(
                contract,
                proxyType,
                (string)null,
                new InjectionConstructor(new ResolvedParameter(contract, uniqueName)));

            return container;
        }
    }
}
