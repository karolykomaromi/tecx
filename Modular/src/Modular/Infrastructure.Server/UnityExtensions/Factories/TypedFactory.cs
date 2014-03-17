namespace Infrastructure.UnityExtensions.Factories
{
    using System;
    using System.Collections.Generic;
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

            InjectionFactory injectionFactory = new InjectionFactory(
                (container, t, n) =>
                {
                    IEnumerable<IInterceptionBehavior> interceptionBehaviors = new[] { new FactoryBehavior(container, this.selector) };

                    IEnumerable<Type> additionalInterfaces = new[] { factoryType };

                    return Intercept.NewInstanceWithAdditionalInterfaces(
                                             typeof(object), 
                                             new VirtualMethodInterceptor(), 
                                             interceptionBehaviors, 
                                             additionalInterfaces);
                });

            injectionFactory.AddPolicies(ignore, factoryType, name, policies);
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