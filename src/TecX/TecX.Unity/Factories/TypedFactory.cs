namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using TecX.Common;
    using TecX.Unity.Proxies;

    public class TypedFactory : InjectionMember
    {
        private const string ProxyGeneratorBuildKey = "ProxyGenerator";

        private readonly ITypedFactoryComponentSelector selector;

        public TypedFactory()
            : this(new DefaultTypedFactoryComponentSelector())
        {
        }

        public TypedFactory(ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(selector, "selector");

            this.selector = selector;
        }

        public override void AddPolicies(Type notNeeded, Type interfaceToProxy, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(interfaceToProxy, "interfaceToProxy");
            Guard.AssertIsInterface(interfaceToProxy, "interfaceToProxy");

            AssertNoMethodHasOutParams(interfaceToProxy);

            IProxyGeneratorPolicy generator = policies.Get<IProxyGeneratorPolicy>(new NamedTypeBuildKey(interfaceToProxy, name));

            if (generator == null)
            {
                generator = new ProxyGeneratorPolicy();

                policies.SetDefault<IProxyGeneratorPolicy>(generator);
            }

            Type dummy = generator.GenerateDummyImplementation(interfaceToProxy);

            var mappingPolicy = new BuildKeyMappingPolicy(new NamedTypeBuildKey(dummy, name));

            policies.Set<IBuildKeyMappingPolicy>(mappingPolicy, new NamedTypeBuildKey(interfaceToProxy, name));

            var interceptor = new Interceptor<InterfaceInterceptor>();

            interceptor.AddPolicies(interfaceToProxy, dummy, name, policies);

            var containerProvider = new InterceptionBehavior<HandContainerDownTheInterceptionPipeline>();

            containerProvider.AddPolicies(interfaceToProxy, dummy, name, policies);

            var factoryInterceptor = new InterceptionBehavior(new FactoryInterceptor(this.selector));
            
            factoryInterceptor.AddPolicies(interfaceToProxy, dummy, name, policies);
        }

        private static void AssertNoMethodHasOutParams(Type factoryType)
        {
            var methods = factoryType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach (var method in methods)
            {
                foreach (var parameter in method.GetParameters())
                {
                    if (parameter.IsOut)
                    {
                        throw new ArgumentException("Methods in factory interface must not have out parameters", "factoryType");
                    }
                }
            }
        }

        private class HandContainerDownTheInterceptionPipeline : IInterceptionBehavior
        {
            private readonly IUnityContainer container;

            public HandContainerDownTheInterceptionPipeline(IUnityContainer container)
            {
                Guard.AssertNotNull(container, "container");

                this.container = container;
            }

            public bool WillExecute
            {
                get
                {
                    return true;
                }
            }

            public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
            {
                input.InvocationContext.Add("container", this.container);

                return getNext()(input, getNext);
            }

            public IEnumerable<Type> GetRequiredInterfaces()
            {
                return Type.EmptyTypes;
            }
        }
    }
}