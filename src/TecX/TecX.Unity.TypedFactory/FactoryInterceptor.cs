using System;
using System.Collections.Generic;
using System.Linq;
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

            //TODO weberse 2011-08-17 create resolver overrides for method parameters
            //ParameterOverrides with name of method parameter and value is value of parameter
            var parameters = new ParameterOverrides();

            var input = invocation.Method.GetParameters();
            for (int i = 0; i < input.Length; i++)
            {
                parameters.Add(input[i].Name, invocation.Arguments[i]);
            }

            //handle collection types
            if (method.ReturnType.IsGenericType)
            {
                var genericReturnType = method.ReturnType.GetGenericTypeDefinition();

                if (genericReturnType == typeof(IEnumerable<>))
                {
                    invocation.ReturnValue = _container.ResolveAll(method.ReturnType, parameters.ToArray());
                    return;
                }

                if (genericReturnType.IsArray)
                {
                    invocation.ReturnValue = _container.ResolveAll(method.ReturnType, parameters.ToArray()).ToArray();
                    return;
                }

                if (genericReturnType == typeof(ICollection<>) ||
                    genericReturnType == typeof(IList<>))
                {
                    invocation.ReturnValue = _container.ResolveAll(method.ReturnType, parameters.ToArray()).ToList();
                    return;
                }
            }

            if (method.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase) &&
                method.Name.Length > 3)
            {
                string tail = method.Name.Substring(3);

                invocation.ReturnValue = _container.Resolve(method.ReturnType, tail, parameters.ToArray());
                return;
            }

            invocation.ReturnValue = _container.Resolve(method.ReturnType, parameters.ToArray());
        }
    }
}