namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class SmartConstructor : InjectionMember
    {
        private readonly List<ConstructorParameter> constructorArguments;

        public SmartConstructor(string argumentName, object value)
            : this()
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.constructorArguments = new List<ConstructorParameter> { new ConstructorParameter(argumentName, value) };
        }

        public SmartConstructor(IEnumerable<ConstructorParameter> constructorArguments)
        {
            Guard.AssertNotNull(constructorArguments, "constructorArguments");

            this.constructorArguments = new List<ConstructorParameter>(constructorArguments);
        }

        public SmartConstructor(params ConstructorParameter[] constructorArguments)
        {
            if (constructorArguments == null)
            {
                this.constructorArguments = new List<ConstructorParameter>();
            }
            else
            {
                this.constructorArguments = new List<ConstructorParameter>(constructorArguments);
            }
        }

        public SmartConstructor With(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.constructorArguments.Add(new ConstructorParameter(value));

            return this;
        }

        public SmartConstructor With(object value, string name)
        {
            Guard.AssertNotEmpty(name, "argumentName");

            this.constructorArguments.Add(new ConstructorParameter(name, value));

            return this;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");
            Guard.AssertNotNull(policies, "policies");

            ConstructorInfo ctor;
            InjectionParameterValue[] parameterValues;

            if (this.constructorArguments.Count == 0)
            {
                ctor = implementationType.GetConstructor(Type.EmptyTypes);

                parameterValues = new InjectionParameterValue[0];
            }
            else
            {
                NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);

                IParameterMatchingConventionsPolicy conventions = policies.Get<IParameterMatchingConventionsPolicy>(key);

                if (conventions == null)
                {
                    conventions = new DefaultMatchingConventionsPolicy();

                    policies.SetDefault<IParameterMatchingConventionsPolicy>(conventions);
                }

                ctor = FindConstructor(implementationType, this.constructorArguments, conventions);

                parameterValues = GetParameterValues(ctor, this.constructorArguments, conventions);
            }

            IConstructorSelectorPolicy policy = new SpecifiedConstructorSelectorPolicy(ctor, parameterValues);

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            policies.Set<IConstructorSelectorPolicy>(policy, buildKey);
        }

        private static ConstructorInfo FindConstructor(Type typeToCreate, IEnumerable<ConstructorParameter> constructorArguments, IParameterMatchingConventionsPolicy conventions)
        {
            ConstructorInfo[] ctors = typeToCreate.GetConstructors();

            ParameterMatcher matcher = new ParameterMatcher(constructorArguments, conventions);

            return matcher.BestMatch(ctors);
        }

        private static InjectionParameterValue[] GetParameterValues(ConstructorInfo ctor, IEnumerable<ConstructorParameter> constructorArguments, IParameterMatchingConventionsPolicy conventions)
        {
            ParameterInfo[] parameters = ctor.GetParameters();

            object[] parameterValues = new object[parameters.Length];

            // fill in the provided values and add the type of the parameter that the
            // ctor expects otherwise. unity will take care of resolving parameters that
            // are provided that way
            for (int i = 0; i < parameters.Length; i++)
            {
                ConstructorParameter argument = constructorArguments.FirstOrDefault(a => conventions.Matches(a, parameters[i]));
                if (argument != null)
                {
                    parameterValues[i] = argument.Value;
                }
                else
                {
                    parameterValues[i] = parameters[i].ParameterType;
                }
            }

            return InjectionParameterValue.ToParameters(parameterValues).ToArray();
        }
    }
}