extern alias CC25;

namespace TecX.Unity.TypedFactory
{
    using CC25.Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class FactoryInterceptor : IInterceptor
    {
        private readonly IUnityContainer container;

        private readonly ITypedFactoryComponentSelector selector;

        public FactoryInterceptor(IUnityContainer container, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(selector, "selector");

            this.container = container;
            this.selector = selector;
        }

        public void Intercept(IInvocation invocation)
        {
            Guard.AssertNotNull(invocation, "invocation");

            var component = this.selector.SelectComponent(invocation.Method, invocation.TargetType, invocation.Arguments);

            invocation.ReturnValue = component.Resolve(this.container);
        }
    }
}