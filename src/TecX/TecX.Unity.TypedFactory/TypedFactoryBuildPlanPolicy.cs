extern alias CC30;

namespace TecX.Unity.TypedFactory
{
    using System;
    using System.Reflection;

    using CC30.Castle.DynamicProxy;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class TypedFactoryBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly ITypedFactoryComponentSelector selector;

        private readonly ProxyGenerator generator;

        public TypedFactoryBuildPlanPolicy(ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(selector, "selector");

            this.selector = selector;
            this.generator = new ProxyGenerator();
        }

        public void BuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            Type factoryType = context.BuildKey.Type; 
            
            Guard.AssertCondition(
                factoryType.IsInterface,
                factoryType,
                "factoryType",
                "Cannot generate an implementation for a non-interface factory type.");

            this.AssertNoMethodHasOutParams(factoryType);

            IUnityContainer container = context.NewBuildUp<IUnityContainer>();

            object proxy = this.generator.CreateInterfaceProxyWithoutTarget(
                factoryType, new FactoryInterceptor(container, this.selector));

            context.Existing = proxy;
        }

        private void AssertNoMethodHasOutParams(Type factoryType)
        {
            var methods = factoryType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach (var method in methods)
            {
                foreach (var parameter in method.GetParameters())
                {
                    if (parameter.IsOut)
                    {
                        throw new ArgumentException("Methods in factory interface must not have out parameters", "TFactory");
                    }
                }
            }
        }
    }
}
