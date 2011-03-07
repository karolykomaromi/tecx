using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class TypeRegistrationExpression<TFrom, TTo> : RegistrationExpression<TypeRegistrationExpression<TFrom, TTo>>
    {
        private readonly List<Func<InjectionMember>> _enrichments;

        protected InjectionMember[] Enrichments
        {
            get
            {
                return _enrichments
                    .Select(enrichment => enrichment())
                    .Where(e => e != null)
                    .ToArray();
            }
        }

        public TypeRegistrationExpression()
        {
            _enrichments = new List<Func<InjectionMember>>();
        }

        public void AddEnrichment(Func<InjectionMember> enrichment)
        {
            Guard.AssertNotNull(enrichment, "enrichment");

            _enrichments.Add(enrichment);
        }

        public TypeRegistrationExpression<TFrom, TTo> ConstructedBy(Func<IUnityContainer, Type, string, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            AddEnrichment(() => new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationExpression<TFrom, TTo> ConstructedBy(Func<IUnityContainer, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            AddEnrichment(() => new InjectionFactory(factoryMethod));

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

                AddEnrichment(() => new InjectionConstructor(parameterTypes.ToArray()));
            }

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(typeof(TFrom), typeof(TTo), null, Lifetime, Enrichments);
        }
    }
}