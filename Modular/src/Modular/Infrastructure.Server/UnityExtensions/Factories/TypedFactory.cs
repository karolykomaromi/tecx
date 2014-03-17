namespace Infrastructure.UnityExtensions.Factories
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class TypedFactory : InjectionMember
    {
        private readonly ITypedFactoryComponentSelector selector;

        public TypedFactory()
            : this(new DefaultTypedFactoryComponentSelector())
        {
        }

        public TypedFactory(ITypedFactoryComponentSelector selector)
        {
            Contract.Requires(selector != null);

            this.selector = selector;
        }

        public override void AddPolicies(Type ignore, Type factoryType, string name, IPolicyList policies)
        {
            AssertNoMethodHasOutParams(factoryType);

            IUnityContainer container = GetContainer(policies);

            IInterceptionBehavior factoryBehavior = new FactoryBehavior(container, this.selector);

            InjectionFactory injectionFactory = new InjectionFactory(
                (c, t, n) => Intercept.NewInstanceWithAdditionalInterfaces(typeof(object), new VirtualMethodInterceptor(), new[] { factoryBehavior }, new[] { factoryType }));

            injectionFactory.AddPolicies(ignore, factoryType, name, policies);
        }

        private static IUnityContainer GetContainer(IPolicyList policies)
        {
            NamedTypeBuildKey key = new NamedTypeBuildKey(typeof(IUnityContainer));

            ILifetimePolicy policy = policies.Get<ILifetimePolicy>(key);

            if (policy == null)
            {
                throw new InvalidOperationException("Could not find the 'ILifetimePolicy' for the current instance of 'IUnityContainer'.");
            }

            IUnityContainer container = policy.GetValue() as IUnityContainer;

            if (container == null)
            {
                throw new InvalidOperationException("Could not get an instance of 'IUnityContainer' from the 'ILifetimePolicy' with the key 'new NamedTypeBuildKey(typeof(IUnityContainer))'");
            }

            return container;
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