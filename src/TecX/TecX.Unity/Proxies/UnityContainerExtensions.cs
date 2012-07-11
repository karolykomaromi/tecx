namespace TecX.Unity.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

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

        public static IUnityContainer RegisterInterfaceProxyWithoutTarget(this IUnityContainer container, Type contract, string name, LifetimeManager lifetime, params IInterceptionBehavior[] behaviors)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(behaviors, "behaviors");

            Interception interception = container.Configure<Interception>();

            if (interception == null)
            {
                container.AddNewExtension<Interception>();
            }

            IProxyGenerator generator = container.Configure<IProxyGenerator>();

            if (generator == null)
            {
                var extension = new ProxyGeneratorExtension();
                generator = extension;
                container.AddExtension(extension);
            }

            List<InjectionMember> injectionMembers = behaviors.Select(b => new InterceptionBehavior(b)).ToList<InjectionMember>();

            injectionMembers.Insert(0, new Interceptor(typeof(InterfaceInterceptor)));

            Type dummy = generator.CreateInterfaceProxyWithoutTargetDummy(contract);

            container.RegisterType(contract, dummy, name, lifetime, injectionMembers.ToArray());

            return container;
        }
    }
}
