namespace Infrastructure.UnityExtensions.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Infrastructure.Reflection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class SmartConstructor : InjectionMember
    {
        private readonly List<Parameter> parameters;

        private readonly IParameterMatchingConvention convention;

        public SmartConstructor(object anonymous)
            : this()
        {
            Contract.Requires(anonymous != null);

            this.parameters = new List<Parameter>();

            if (TypeHelper.IsAnonymous(anonymous.GetType()))
            {
                IEnumerable<PropertyInfo> properties = TypeHelper.GetPublicProperties(anonymous);

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
            : this()
        {
            Contract.Requires(!string.IsNullOrEmpty(argumentName));

            this.parameters = new List<Parameter> { new Parameter(argumentName, value) };
        }

        public SmartConstructor(params Parameter[] constructorArguments)
            : this()
        {
            this.parameters = new List<Parameter>(constructorArguments ?? new Parameter[0]);
        }

        private SmartConstructor()
        {
            this.convention = new CompositeConvention(
                                new StringAsMappingNameConvention(),
                                new SpecifiedNameConvention(),
                                new ConnectionStringConvention(),
                                new FileNameConvention(),
                                new ByTypeConvention());
        }

        public static ConstructorInfo FindConstructor(Type typeToCreate, IEnumerable<Parameter> parameters, IParameterMatchingConvention convention)
        {
            ConstructorInfo[] ctors = typeToCreate.GetConstructors().OrderByDescending(ctor => ctor.GetParameters().Length).ToArray();
            
            ParameterMatcher matcher = new ParameterMatcher(parameters, convention);

            return matcher.BestMatch(ctors);
        }

        public static InjectionParameterValue[] GetParameterValues(ConstructorInfo ctor, ICollection<Parameter> parameters, IParameterMatchingConvention convention)
        {
            ParameterInfo[] arguments = ctor.GetParameters();

            object[] parameterValues = new object[arguments.Length];

            // fill in the provided values and add the type of the parameter that the
            // ctor expects otherwise. unity will take care of resolving parameters that
            // are provided that way
            for (int i = 0; i < arguments.Length; i++)
            {
                Type parameterType = arguments[i].ParameterType;
                string parameterName = arguments[i].Name;

                Parameter parameter = parameters.FirstOrDefault(arg => convention.IsMatch(arg, arguments[i]));
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
                    if (IsAbstractAndTypeNameDoesntStartWithParamName(parameterType, parameterName) ||
                        IsInterfaceAndTypeNameDoesntStartWithIPlusParamName(parameterType, parameterName) ||
                        IsClassNotStringAndTypeNameDoesntStartWithParamName(parameterType, parameterName))
                    {
                        parameterValues[i] = new ResolvedParameter(parameterType, parameterName);
                    }
                    else
                    {
                        parameterValues[i] = arguments[i].ParameterType;
                    }
                }
            }

            return InjectionParameterValue.ToParameters(parameterValues).ToArray();
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
                ctor = FindConstructor(implementationType, this.parameters, this.convention);

                if (ctor == null)
                {
                    ctor = implementationType.GetConstructor(Type.EmptyTypes);

                    if (ctor == null)
                    {
                        throw new ConstructorNotFoundException(implementationType);
                    }

                    parameterValues = new InjectionParameterValue[0];
                }
                else
                {
                    parameterValues = GetParameterValues(ctor, this.parameters, this.convention);
                }
            }
            else
            {
                ctor = FindConstructor(implementationType, this.parameters, this.convention);

                parameterValues = GetParameterValues(ctor, this.parameters, this.convention);
            }

            IConstructorSelectorPolicy policy = new SpecifiedConstructorSelectorPolicy(ctor, parameterValues);

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            policies.Set<IConstructorSelectorPolicy>(policy, buildKey);
        }

        private static bool IsClassNotStringAndTypeNameDoesntStartWithParamName(Type parameterType, string paramName)
        {
            bool isMatch = parameterType.IsClass && parameterType != typeof(string) && !parameterType.Name.StartsWith(paramName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }

        private static bool IsInterfaceAndTypeNameDoesntStartWithIPlusParamName(Type parameterType, string paramName)
        {
            bool isMatch = parameterType.IsInterface && !parameterType.Name.StartsWith("I" + paramName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }

        private static bool IsAbstractAndTypeNameDoesntStartWithParamName(Type parameterType, string paramName)
        {
            bool isMatch = !parameterType.IsInterface && parameterType.IsAbstract && !parameterType.Name.StartsWith(paramName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }
    }
}