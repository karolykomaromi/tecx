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

    public class ClozeInjectionConstructor : InjectionMember
    {
        private readonly ConstructorArgumentCollection constructorArguments;

        public ClozeInjectionConstructor(string argumentName, object value)
            : this()
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.constructorArguments.Add(new ConstructorArgument(argumentName, value));
        }

        public ClozeInjectionConstructor(IEnumerable<ConstructorArgument> ctorArguments)
            : this()
        {
            Guard.AssertNotNull(ctorArguments, "ctorArguments");

            foreach (var ctorArgument in ctorArguments)
            {
                this.constructorArguments.Add(ctorArgument);
            }
        }

        private ClozeInjectionConstructor()
        {
            this.constructorArguments = new ConstructorArgumentCollection();
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            ConstructorInfo ctor = this.FindConstructor(implementationType);

            InjectionParameterValue[] parameterValues = this.GetParameterValues(ctor);

            policies.Set<IConstructorSelectorPolicy>(
                new SpecifiedConstructorSelectorPolicy(ctor, parameterValues),
                new NamedTypeBuildKey(implementationType, name));
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