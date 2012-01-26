namespace TecX.Unity.Configuration.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Enrichment;
    using TecX.Unity.Injection;

    public class TypeRegistrationExpression : RegistrationExpression<TypeRegistrationExpression>
    {
        private readonly InjectionMembers enrichments;
        private readonly Type @from;
        private readonly Type to;

        public TypeRegistrationExpression(Type from, Type to)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            this.@from = from;
            this.to = to;

            this.enrichments = new InjectionMembers();
        }

        protected Type From
        {
            get { return this.@from; }
        }

        protected Type To
        {
            get { return this.to; }
        }

        protected InjectionMember[] Enrichments
        {
            get
            {
                return this.enrichments.ToArray();
            }
        }

        public TypeRegistrationExpression EnrichWith<T>(Action<T, IBuilderContext> action)
            where T : class
        {
            Guard.AssertNotNull(action, "action");

            this.enrichments.Add(new Enrichment<T>(action));

            return this;
        }

        public TypeRegistrationExpression EnrichWith(Action<InjectionMembers> action)
        {
            Guard.AssertNotNull(action, "action");

            action(this.enrichments);

            return this;
        }

        public TypeRegistrationExpression CreatedUsing(Func<IUnityContainer, Type, string, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            this.enrichments.Add(new InjectionFactory(factoryMethod));

            return this;
        }

        public TypeRegistrationExpression CreatedUsing(Func<IUnityContainer, object> factoryMethod)
        {
            Guard.AssertNotNull(factoryMethod, "factoryMethod");

            this.enrichments.Add(new InjectionFactory(factoryMethod));

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

                this.enrichments.Add(new InjectionConstructor(parameterTypes.ToArray()));
            }

            return this;
        }

        public TypeRegistrationExpression DefaultCtor()
        {
            this.enrichments.Add(new InjectionConstructor());

            return this;
        }

        public TypeRegistrationExpression Property(string propertyName)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            this.enrichments.Add(new InjectionProperty(propertyName));

            return this;
        }

        public TypeRegistrationExpression Property(string propertyName, object value)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");
            Guard.AssertNotNull(value, "value");

            this.enrichments.Add(new InjectionProperty(propertyName, value));

            return this;
        }

        public TypeRegistrationExpression Method(string methodName, params object[] args)
        {
            Guard.AssertNotEmpty(methodName, "methodName");

            this.enrichments.Add(new InjectionMethod(methodName, args));

            return this;
        }

        public TypeRegistrationExpression DependsOn(object anonymous)
        {
            Guard.AssertNotNull(anonymous, "anonymous");

            var properties = anonymous.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var arguments = new ConstructorArgumentCollection();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var ctors = this.To.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

                    var ctor = ctors.FirstOrDefault(c => c.GetParameters().Any(p => p.Name == property.Name));

                    if (ctor != null)
                    {
                        var parameter = ctor.GetParameters().Single(p => p.Name == property.Name);

                        var argument = new ConstructorArgument(
                            property.Name, new ResolvedParameter(parameter.ParameterType, (string)property.GetValue(anonymous, null)));

                        arguments.Add(argument);
                    }
                }
            }

            this.enrichments.Add(new ClozeInjectionConstructor(arguments));

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(this.From, this.To, null, this.Lifetime, this.Enrichments);
        }
    }
}