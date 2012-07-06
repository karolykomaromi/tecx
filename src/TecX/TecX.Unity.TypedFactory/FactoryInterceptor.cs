namespace TecX.Unity.TypedFactory
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
        private readonly IUnityContainer container;

        private readonly ITypedFactoryComponentSelector selector;

        public FactoryInterceptor(IUnityContainer container)
            : this(container, new DefaultTypedFactoryComponentSelector())
        {
        }

        protected FactoryInterceptor(IUnityContainer container, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(selector, "selector");

            this.container = container;
            this.selector = selector;
        }

        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        //public void Intercept(IInvocation invocation)
        //{
        //    Guard.AssertNotNull(invocation, "invocation");

        //    var component = this.selector.SelectComponent(invocation.Method, invocation.TargetType, invocation.Arguments);

        //    invocation.ReturnValue = component.Resolve(this.container);
        //}

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Guard.AssertNotNull(input, "input");

            var component = this.selector.SelectComponent((MethodInfo)input.MethodBase, input.MethodBase.DeclaringType, input.Arguments.OfType<object>().ToArray());

            return input.CreateMethodReturn(component.Resolve(this.container));
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}