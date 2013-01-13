namespace TecX.Unity.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using TecX.Common;

    public class FactoryInterceptor : IInterceptionBehavior
    {
        private readonly ITypedFactoryComponentSelector selector;

        public FactoryInterceptor(ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(selector, "selector");

            this.selector = selector;
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
            Guard.AssertNotNull(input, "input");

            // can't intercept calls to constructor anyway so why do they use MethodBase instead of MethodInfo?
            var component = this.selector.SelectComponent((MethodInfo)input.MethodBase, input.MethodBase.DeclaringType, input.Arguments.OfType<object>().ToArray());

            IUnityContainer container = null;

            if (input.InvocationContext.ContainsKey("container"))
            {
                container = input.InvocationContext["container"] as IUnityContainer;
            }

            if (container == null)
            {
                throw new InvalidOperationException(
                    "Could not find an IUnityContainer in 'input.InvocationContext'. " +
                    "The TypedFactory must add an IInterceptionBehavior to the pipeline that provides that container instance with 'container' as its key.");
            }

            object resolvedObject = component.Resolve(container);

            return input.CreateMethodReturn(resolvedObject);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}