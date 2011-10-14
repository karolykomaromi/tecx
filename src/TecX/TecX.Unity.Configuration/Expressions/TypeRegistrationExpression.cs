using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class TypeRegistrationExpression : RegistrationExpression<TypeRegistrationExpression>
    {
        private readonly InjectionMembers _enrichments;
        private readonly Type _from;
        private readonly Type _to;

        protected Type From
        {
            get { return _from; }
        }

        protected Type To
        {
            get { return _to; }
        }

        protected InjectionMember[] Enrichments
        {
            get
            {
                return _enrichments.ToArray();
            }
        }

        public TypeRegistrationExpression(Type from, Type to)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            _from = from;
            _to = to;

            _enrichments = new InjectionMembers();
        }

        public TypeRegistrationExpression EnrichWith(Action<InjectionMembers> action)
        {
            Guard.AssertNotNull(action, "action");

            action(_enrichments);

            return this;
        }

        public TypeRegistrationExpression CreatedUsing(Func<IUnityContainer, Type, string, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            _enrichments.Add(new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationExpression CreatedUsing(Func<IUnityContainer, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            _enrichments.Add(new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationExpression Ctor(Expression<Func<object>> expression)
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

                _enrichments.Add(new InjectionConstructor(parameterTypes.ToArray()));
            }

            return this;
        }

        public TypeRegistrationExpression DefaultCtor()
        {
            _enrichments.Add(new InjectionConstructor());

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(From, To, null, Lifetime, Enrichments);
        }
    }
}