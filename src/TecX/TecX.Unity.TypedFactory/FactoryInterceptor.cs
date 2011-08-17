using Castle.DynamicProxy;
using Microsoft.Practices.Unity;
using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class FactoryInterceptor : IInterceptor
    {
        private readonly IUnityContainer _container;

        public FactoryInterceptor(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            _container = container;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            if(method.Name == "Create")
            {
                invocation.ReturnValue = _container.Resolve(method.ReturnType);
            }
        }
    }
}