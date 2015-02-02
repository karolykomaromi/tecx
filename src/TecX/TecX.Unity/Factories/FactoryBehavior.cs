namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using TecX.Common;

    public class FactoryBehavior : IInterceptionBehavior
    {
        private readonly IUnityContainer container;
        private readonly ITypedFactoryComponentSelector selector;

        public FactoryBehavior(IUnityContainer container, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(selector, "selector");

            this.container = container;
            this.selector = selector;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Guard.AssertNotNull(input, "input");

            // can't intercept calls to constructor anyway so why do they use MethodBase instead of MethodInfo?
            MethodInfo method = input.MethodBase as MethodInfo;

            if (method == null)
            {
                return getNext()(input, getNext);
            }

            var component = this.selector.SelectComponent(method, method.DeclaringType, Enumerable.OfType<object>(input.Arguments).ToArray());

            object resolvedObject = component.Resolve(this.container);

            return input.CreateMethodReturn(resolvedObject);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}