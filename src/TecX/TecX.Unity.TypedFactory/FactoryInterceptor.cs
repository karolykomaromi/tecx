using Castle.DynamicProxy;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class FactoryInterceptor : IInterceptor
    {
        private readonly IUnityContainer _container;

        private readonly ITypedFactoryComponentSelector _selector;

        public FactoryInterceptor(IUnityContainer container, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(selector, "selector");

            _container = container;
            _selector = selector;
        }

        public void Intercept(IInvocation invocation)
        {
            Guard.AssertNotNull(invocation, "invocation");

            var component = _selector.SelectComponent(invocation.Method, invocation.TargetType, invocation.Arguments);

            invocation.ReturnValue = component.Resolve(_container);
        }
    }
}