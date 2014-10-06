namespace TecX.Unity.Factories
{
    using System;
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using TecX.Common;


    public class TypedFactory : InjectionMember
    {
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

        public override void AddPolicies(Type ignore, Type factoryType, string name, IPolicyList policies)
        {
            AssertNoMethodHasOutParams(factoryType);
            
            if (factoryType.IsInterface)
            {
                InjectionFactory injectionFactory = new InjectionFactory(
                    (container, t, n) => Intercept.NewInstanceWithAdditionalInterfaces(
                        typeof(object),
                        new VirtualMethodInterceptor(),
                        new IInterceptionBehavior[] { new FactoryBehavior(container, this.selector) },
                        new[] { factoryType }));

                injectionFactory.AddPolicies(ignore, factoryType, name, policies);
            }
            else if (factoryType.IsAbstract)
            {
                InjectionFactory injectionFactory = new InjectionFactory(
                    (container, t, n) => Intercept.NewInstance(
                        factoryType,
                        new VirtualMethodInterceptor(),
                        new IInterceptionBehavior[] { new FactoryBehavior(container, this.selector) }));

                injectionFactory.AddPolicies(ignore, factoryType, name, policies);
            }
            else
            {
                throw new ArgumentException("'factoryType' must either be an interface or an abstract class.", "factoryType");
            }
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
                        string msg = string.Format(
                                "Parameter '{0}' of method '{1}.{2}' is an 'out' parameter. No method of your factory type is allowed to use an 'out' parameter.",
                                parameter.Name,
                                factoryType.FullName,
                                method.Name);

                        throw new ArgumentException(msg, "factoryType");
                    }
                }
            }
        }
    }
}