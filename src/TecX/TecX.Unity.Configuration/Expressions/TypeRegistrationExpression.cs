using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class TypeRegistrationExpression<TFrom, TTo> : RegistrationExpression<TypeRegistrationExpression<TFrom, TTo>>
    {
        public TypeRegistrationExpression<TFrom, TTo> ConstructedBy(Func<IUnityContainer, Type, string, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            InjectionFactory factory = new InjectionFactory(factoryMethod);

            return this;
        }

        public TypeRegistrationExpression<TFrom, TTo> ConstructedBy(Func<IUnityContainer, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            InjectionFactory factory = new InjectionFactory(factoryMethod);

            return this;
        }

        public TypeRegistrationExpression<TFrom, TTo> SelectConstructor(Expression<Func<TTo>> expression)
        {
            Guard.AssertNotNull(expression, "expression");

            NewExpression nx = expression.Body as NewExpression;

            if (nx != null)
            {
                ConstructorInfo ctor = nx.Constructor;

                List<Type> parameterTypes = new List<Type>();

                foreach (ParameterInfo parameterInfo in ctor.GetParameters())
                {
                    parameterTypes.Add(parameterInfo.ParameterType);
                }

                InjectionConstructor ic = new InjectionConstructor(parameterTypes.ToArray());
            }

            return this;
        }

        public override Registration Compile()
        {
            throw new NotImplementedException();
        }
    }
}