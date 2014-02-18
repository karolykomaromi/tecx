namespace Infrastructure.UnityExtensions.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Conventions;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class SmartConstructor : InjectionMember
    {
        private readonly List<Parameter> parameters;

        public SmartConstructor(object anonymous)
        {
            Contract.Requires(anonymous != null);

            this.parameters = new List<Parameter>();

            if (TypeHelper.IsAnonymous(anonymous.GetType()))
            {
                var properties = TypeHelper.GetPublicProperties(anonymous);

                foreach (var property in properties)
                {
                    this.parameters.Add(new Parameter(property.Name, property.GetValue(anonymous, null)));
                }
            }
            else
            {
                this.parameters.Add(new Parameter(anonymous));
            }
        }

        public SmartConstructor(string argumentName, object value)
        {
            Contract.Requires(!string.IsNullOrEmpty(argumentName));

            this.parameters = new List<Parameter> { new Parameter(argumentName, value) };
        }

        public SmartConstructor(IEnumerable<Parameter> constructorArguments)
        {
            Contract.Requires(constructorArguments != null);

            this.parameters = new List<Parameter>(constructorArguments);
        }

        public SmartConstructor(params Parameter[] constructorArguments)
        {
            this.parameters = new List<Parameter>(constructorArguments ?? new Parameter[0]);
        }

        public SmartConstructor With(object value)
        {
            Contract.Requires(value != null);

            this.parameters.Add(new Parameter(value));

            return this;
        }

        public SmartConstructor With(string name, object value)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            this.parameters.Add(new Parameter(name, value));

            return this;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Contract.Requires(implementationType != null);
            Contract.Requires(policies != null);

            ConstructorInfo ctor;
            InjectionParameterValue[] parameterValues;

            if (this.parameters.Count == 0)
            {
                ctor = implementationType.GetConstructor(Type.EmptyTypes);

                parameterValues = new InjectionParameterValue[0];
            }
            else
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);

                IParameterMatchingPolicy conventions = policies.Get<IParameterMatchingPolicy>(key);

                if (conventions == null)
                {
                    conventions = new ParameterMatchingPolicy();

                    policies.SetDefault<IParameterMatchingPolicy>(conventions);
                }

                ctor = FindConstructor(implementationType, this.parameters, conventions);

                parameterValues = GetParameterValues(ctor, this.parameters, conventions);
            }

            IConstructorSelectorPolicy policy = new SpecifiedConstructorSelectorPolicy(ctor, parameterValues);

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            policies.Set<IConstructorSelectorPolicy>(policy, buildKey);
        }

        private static ConstructorInfo FindConstructor(Type typeToCreate, IEnumerable<Parameter> constructorArguments, IParameterMatchingPolicy conventions)
        {
            Contract.Requires(typeToCreate != null);
            Contract.Requires(constructorArguments != null);
            Contract.Requires(conventions != null);

            ConstructorInfo[] ctors = typeToCreate.GetConstructors();

            ParameterMatcher matcher = new ParameterMatcher(constructorArguments, conventions);

            return matcher.BestMatch(ctors);
        }

        private static InjectionParameterValue[] GetParameterValues(ConstructorInfo ctor, ICollection<Parameter> constructorArguments, IParameterMatchingPolicy conventions)
        {
            Contract.Requires(ctor != null);
            Contract.Requires(constructorArguments != null);
            Contract.Requires(conventions != null);

            ParameterInfo[] parameters = ctor.GetParameters();

            object[] parameterValues = new object[parameters.Length];

            // fill in the provided values and add the type of the parameter that the
            // ctor expects otherwise. unity will take care of resolving parameters that
            // are provided that way
            for (int i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;
                string parameterName = parameters[i].Name;

                Parameter parameter = constructorArguments.FirstOrDefault(a => conventions.Matches(a, parameters[i]));
                if (parameter != null)
                {

                    if (parameter.Value is string &&
                        parameterType != typeof(string) &&
                        (parameterType.IsInterface || parameterType.IsClass))
                    {
                        parameterValues[i] = new ResolvedParameter(parameterType, (string)parameter.Value);
                    }
                    else
                    {
                        parameterValues[i] = parameter.Value;
                    }
                }
                else
                {
                    if (MapToRegistrationNamesStrategy.IsAbstractAndTypeNameDoesntStartWithParamName(parameterType, parameterName) ||
                        MapToRegistrationNamesStrategy.IsInterfaceAndTypeNameDoesntStartWithIPlusParamName(parameterType, parameterName) ||
                        MapToRegistrationNamesStrategy.IsClassNotStringAndTypeNameDoesntStartWithParamName(parameterType, parameterName))
                    {
                        parameterValues[i] = new ResolvedParameter(parameterType, parameterName);
                    }
                    else
                    {
                        parameterValues[i] = parameters[i].ParameterType;
                    }
                }
            }

            return InjectionParameterValue.ToParameters(parameterValues).ToArray();
        }
    }
}