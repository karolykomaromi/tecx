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
        //private readonly IUnityContainer container;

        private readonly ITypedFactoryComponentSelector selector;

        //public FactoryInterceptor(IUnityContainer container)
        //    : this(container, new DefaultTypedFactoryComponentSelector())
        //{
        //}

        ///// <summary>
        ///// This constructor is protected on purpose! Unity is used to resolve this interceptor and
        ///// I didn't want to have to register a mapping for the selector but make it possible to use
        ///// an alternative selector.
        ///// </summary>
        //protected FactoryInterceptor(IUnityContainer container, ITypedFactoryComponentSelector selector)
        //{
        //    Guard.AssertNotNull(container, "container");
        //    Guard.AssertNotNull(selector, "selector");

        //    this.container = container;
        //    this.selector = selector;
        //}

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

            var component = this.selector.SelectComponent((MethodInfo)input.MethodBase, input.MethodBase.DeclaringType, input.Arguments.OfType<object>().ToArray());

            IUnityContainer container = input.InvocationContext["container"] as IUnityContainer;

            object x = component.Resolve(container);

            //return input.CreateMethodReturn(component.Resolve(this.container));

            return input.CreateMethodReturn(x);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}