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
        private readonly List<ConstructorArgument> constructorArguments;

        public SmartConstructor(string argumentName, object value)
            : this()
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.constructorArguments = new List<ConstructorArgument> { new ConstructorArgument(argumentName, value) };
        }

        public SmartConstructor(IEnumerable<ConstructorArgument> constructorArguments)
        {
            Guard.AssertNotNull(constructorArguments, "constructorArguments");

            this.constructorArguments = new List<ConstructorArgument>(constructorArguments);
        }

        public SmartConstructor(params ConstructorArgument[] constructorArguments)
        {
            if (constructorArguments == null)
            {
                this.constructorArguments = new List<ConstructorArgument>();
            }
            else
            {
                this.constructorArguments = new List<ConstructorArgument>(constructorArguments);
            }
        }

        public SmartConstructor With(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.constructorArguments.Add(new ConstructorArgument(value));

            return this;
        }

        public SmartConstructor With(object value, string name)
        {
            Guard.AssertNotEmpty(name, "argumentName");

            this.constructorArguments.Add(new ConstructorArgument(name, value));

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

                IArgumentMatchingConventionsPolicy conventions = policies.Get<IArgumentMatchingConventionsPolicy>(key);

                if (conventions == null)
                {
                    conventions = new DefaultMatchingConventionsPolicy();

                    policies.SetDefault<IArgumentMatchingConventionsPolicy>(conventions);
                }

                ctor = FindConstructor(implementationType, this.constructorArguments, conventions);

                parameterValues = GetParameterValues(ctor, this.constructorArguments, conventions);
            }

            IConstructorSelectorPolicy policy = new SpecifiedConstructorSelectorPolicy(ctor, parameterValues);

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            policies.Set<IConstructorSelectorPolicy>(policy, buildKey);
        }

        private static ConstructorInfo FindConstructor(Type typeToCreate, IEnumerable<ConstructorArgument> constructorArguments, IArgumentMatchingConventionsPolicy conventions)
        {
            ConstructorInfo[] ctors = typeToCreate.GetConstructors();

            ParameterMatcher matcher = new ParameterMatcher(constructorArguments, conventions);

            return matcher.BestMatch(ctors);
        }

        private static InjectionParameterValue[] GetParameterValues(ConstructorInfo ctor, IEnumerable<ConstructorArgument> constructorArguments, IArgumentMatchingConventionsPolicy conventions)
        {
            ParameterInfo[] parameters = ctor.GetParameters();

            object[] parameterValues = new object[parameters.Length];

            // fill in the provided values and add the type of the parameter that the
            // ctor expects otherwise. unity will take care of resolving parameters that
            // are provided that way
            for (int i = 0; i < parameters.Length; i++)
            {
                ConstructorArgument argument = constructorArguments.FirstOrDefault(a => conventions.Matches(a, parameters[i]));
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