using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class TypeRegistrationExpression : RegistrationExpression<TypeRegistrationExpression>
    {
        #region Fields

        private readonly List<Func<InjectionMember>> _enrichments;
        private readonly Type _from;
        private readonly Type _to;

        #endregion Fields

        #region Properties

        protected Type From { get { return _from; } }

        protected Type To { get { return _to; } }

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

        #endregion Properties

        #region c'tor

        public TypeRegistrationExpression(Type from, Type to)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            _from = from;
            _to = to;

            _enrichments = new List<Func<InjectionMember>>();
        }

        #endregion c'tor

        public void AddEnrichment(Func<InjectionMember> enrichment)
        {
            Guard.AssertNotNull(enrichment, "enrichment");

            _enrichments.Add(enrichment);
        }

        public TypeRegistrationExpression ConstructedBy(Func<IUnityContainer, Type, string, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            AddEnrichment(() => new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationExpression ConstructedBy(Func<IUnityContainer, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            AddEnrichment(() => new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationExpression SelectConstructor(Expression<Func<object>> expression)
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
            return new TypeRegistration(_from, _to, null, Lifetime, Enrichments);
        }
    }
}