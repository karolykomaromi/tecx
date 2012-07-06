namespace TecX.Unity.TypedFactory
{
    using System;
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

        ////public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        ////{
        ////    Guard.AssertNotNull(implementationType, "implementationType");
        ////    Guard.AssertNotNull(policies, "policies");

        ////    var policy = new TypedFactoryBuildPlanPolicy(this.selector);

        ////    policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        ////}

        public override void AddPolicies(Type notNeeded, Type interfaceToProxy, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(interfaceToProxy, "interfaceToProxy");
            Guard.AssertIsInterface(interfaceToProxy, "interfaceToProxy");

            AssertNoMethodHasOutParams(interfaceToProxy);

            IProxyGeneratorPolicy generator = policies.Get<IProxyGeneratorPolicy>(ProxyGeneratorBuildKey);

            if (generator == null)
            {
                generator = new ProxyGeneratorPolicy();

                policies.Set<IProxyGeneratorPolicy>(generator, ProxyGeneratorBuildKey);
            }

            Type dummy = generator.GenerateDummyImplementation(interfaceToProxy);

            var mappingPolicy = new BuildKeyMappingPolicy(new NamedTypeBuildKey(dummy, name));

            policies.Set<IBuildKeyMappingPolicy>(mappingPolicy, new NamedTypeBuildKey(interfaceToProxy, name));

            var interceptor = new Interceptor<InterfaceInterceptor>();

            interceptor.AddPolicies(interfaceToProxy, dummy, name, policies);

            var behavior = new InterceptionBehavior<FactoryInterceptor>();

            behavior.AddPolicies(interfaceToProxy, dummy, name, policies);
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
    }
}