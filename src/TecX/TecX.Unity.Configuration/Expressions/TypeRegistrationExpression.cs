﻿using System;
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

        private readonly List<InjectionMember> _enrichments;
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
                return _enrichments.ToArray();
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

            _enrichments = new List<InjectionMember>();
        }

        #endregion c'tor

        public TypeRegistrationExpression EnrichWith(Action<InjectionMembers> action)
        {
            Guard.AssertNotNull(() => action);

            InjectionMembers injectionMembers = new InjectionMembers();

            action(injectionMembers);

            _enrichments.Add(injectionMembers);

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

        public TypeRegistrationExpression UsingConstructor(Expression<Func<object>> expression)
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

        public TypeRegistrationExpression UsingDefaultConstructor()
        {
            _enrichments.Add(new InjectionConstructor());

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(_from, _to, null, Lifetime, Enrichments);
        }
    }
}