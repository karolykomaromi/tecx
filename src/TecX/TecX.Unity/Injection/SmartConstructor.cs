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

        public SmartConstructor With(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.constructorArguments.Add(new ConstructorArgument(argumentName, value));

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
                ctor = this.FindConstructor(implementationType);

                parameterValues = this.GetParameterValues(ctor);
            }

            IConstructorSelectorPolicy policy = new SpecifiedConstructorSelectorPolicy(ctor, parameterValues);

            NamedTypeBuildKey buildKey = new NamedTypeBuildKey(implementationType, name);

            policies.Set<IConstructorSelectorPolicy>(policy, buildKey);
        }

        private InjectionParameterValue[] GetParameterValues(ConstructorInfo ctor)
        {
            ParameterInfo[] infos = ctor.GetParameters();

            object[] parameterValues = new object[infos.Length];

            // fill in the provided values and add the type of the parameter that the
            // ctor expects otherwise. unity will take care of resolving parameters that
            // are provided that way
            for (int i = 0; i < infos.Length; i++)
            {
                ConstructorArgument argument;
                if (this.constructorArguments.TryGetArgumentByName(infos[i].Name, out argument))
                {
                    parameterValues[i] = argument.Value;
                }
                else
                {
                    parameterValues[i] = infos[i].ParameterType;
                }
            }

            return InjectionParameterValue.ToParameters(parameterValues).ToArray();
        }

        private ConstructorInfo FindConstructor(Type typeToCreate)
        {
            var ctors = typeToCreate.GetConstructors();

            ParameterMatcher matcher = new ParameterMatcher(this.constructorArguments);

            return matcher.BestMatch(ctors);
        }
    }
}