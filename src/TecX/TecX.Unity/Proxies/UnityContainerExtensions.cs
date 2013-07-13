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

            ProxyGenerator generator = GetOrAddProxyGenerator(container);

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

            ProxyGenerator generator = GetOrAddProxyGenerator(container);

            List<InjectionMember> injectionMembers = behaviors.Select(b => new InterceptionBehavior(b)).ToList<InjectionMember>();

            injectionMembers.Insert(0, new Interceptor(typeof(InterfaceInterceptor)));

            Type dummy = generator.CreateInterfaceProxyWithoutTargetDummy(contract);

            container.RegisterType(contract, dummy, name, lifetime, injectionMembers.ToArray());

            return container;
        }

        public static IUnityContainer RegisterNullObject(this IUnityContainer container, Type contract, string name, LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(contract, "contract");
            //Guard.AssertIsInterface(contract, "contract");

            ProxyGenerator generator = GetOrAddProxyGenerator(container);

            Type nullObject = generator.CreateNullObject(contract);

            container.RegisterType(contract, nullObject, name, lifetime, injectionMembers);

            return container;
        }

        private static ProxyGenerator GetOrAddProxyGenerator(IUnityContainer container)
        {
            ProxyGenerator generator = container.Configure<ProxyGenerator>();

            if (generator == null)
            {
                generator = new ProxyGenerator();
                container.AddExtension(generator);
            }

            return generator;
        }
    }
}
