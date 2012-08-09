namespace TecX.Unity.Configuration.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration.Utilities;
    using TecX.Unity.Enrichment;
    using TecX.Unity.Factories;
    using TecX.Unity.Injection;

    public class TypeRegistrationBuilder : RegistrationBuilder<TypeRegistrationBuilder>
    {
        private readonly InjectionMembers enrichments;

        private readonly Type to;

        public TypeRegistrationBuilder(Type from, Type to)
            : base(@from)
        {
            Guard.AssertNotNull(to, "to");

            this.to = to;

            this.enrichments = new InjectionMembers();
        }

        public Type To
        {
            get { return this.to; }
        }

        public InjectionMember[] Enrichments
        {
            get
            {
                return this.enrichments.ToArray();
            }
        }

        public TypeRegistrationBuilder Enrich<T>(Action<T, IBuilderContext> action)
            where T : class
        {
            Guard.AssertNotNull(action, "action");

            this.enrichments.Add(new Enrichment<T>(action));

            return this;
        }

        public TypeRegistrationBuilder Enrich(Action<InjectionMembers> action)
        {
            Guard.AssertNotNull(action, "action");

            action(this.enrichments);

            return this;
        }

        public TypeRegistrationBuilder Factory(Func<IUnityContainer, Type, string, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            this.enrichments.Add(new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationBuilder Factory(Func<IUnityContainer, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            this.enrichments.Add(new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationBuilder Ctor(params ConstructorParameter[] parameterValues)
        {
            SmartConstructor ctor = new SmartConstructor(parameterValues);

            this.enrichments.Add(ctor);

            return this;
        }

        public TypeRegistrationBuilder Ctor(params object[] parameterValues)
        {
            InjectionConstructor ctor = new InjectionConstructor(parameterValues);

            this.enrichments.Add(ctor);

            return this;
        }

        public TypeRegistrationBuilder Ctor(Expression<Func<object>> expression)
        {
            Guard.AssertNotNull(expression, "builder");

            NewExpression nx = expression.Body as NewExpression;

            if (nx != null)
            {
                ConstructorInfo ctor = nx.Constructor;

                List<Type> parameterTypes = new List<Type>();

                foreach (ParameterInfo parameterInfo in ctor.GetParameters())
                {
                    parameterTypes.Add(parameterInfo.ParameterType);
                }

                this.enrichments.Add(new InjectionConstructor(parameterTypes.ToArray()));
            }

            return this;
        }

        public TypeRegistrationBuilder DefaultCtor()
        {
            this.enrichments.Add(new InjectionConstructor());

            return this;
        }

        public TypeRegistrationBuilder Property(string propertyName)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            this.enrichments.Add(new InjectionProperty(propertyName));

            return this;
        }

        public TypeRegistrationBuilder Property(string propertyName, object value)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");
            Guard.AssertNotNull(value, "value");

            this.enrichments.Add(new InjectionProperty(propertyName, value));

            return this;
        }

        public TypeRegistrationBuilder Method(string methodName, params object[] args)
        {
            Guard.AssertNotEmpty(methodName, "methodName");

            this.enrichments.Add(new InjectionMethod(methodName, args));

            return this;
        }

        public TypeRegistrationBuilder Override(object anonymous)
        {
            Guard.AssertNotNull(anonymous, "anonymous");

            var overrides = new SmartConstructor(anonymous);

            this.enrichments.Add(overrides);

            return this;
        }

        public override Registration Build()
        {
            if (this.Predicate == null)
            {
                return new TypeRegistration(this.From, this.To, this.Name, this.Lifetime, this.Enrichments);
            }

            return new ContextualTypeRegistration(this.From, this.To, this.Name, this.Lifetime, this.Predicate, this.Enrichments);
        }
    }
}