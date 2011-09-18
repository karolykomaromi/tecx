using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

using TecX.Common;

namespace TecX.Unity.Injection
{
    public class ClozeInjectionConstructur : InjectionMember
    {
        private readonly ConstructorArgumentCollection _constructorArguments;

        private ClozeInjectionConstructur()
        {
            _constructorArguments = new ConstructorArgumentCollection();
        }

        public ClozeInjectionConstructur(string argumentName, object value)
            : this()
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            _constructorArguments.Add(new ConstructorArgument(argumentName, value));
        }

        public ClozeInjectionConstructur(IEnumerable<ConstructorArgument> ctorArguments)
            : this()
        {
            Guard.AssertNotNull(ctorArguments, "ctorArguments");

            foreach (var ctorArgument in ctorArguments)
            {
                _constructorArguments.Add(ctorArgument);
            }
        }

        public override void AddPolicies(Type serviceType, 
            Type implementationType, 
            string name,
            IPolicyList policies)
        {
            ConstructorInfo ctor = FindConstructor(implementationType);

            InjectionParameterValue[] parameterValues = GetParameterValues(ctor);

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
                if (_constructorArguments.TryGetArgumentByName(infos[i].Name, out argument))
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

            ParameterMatcher matcher = new ParameterMatcher(_constructorArguments);

            return matcher.BestMatch(ctors);
        }
    }
}