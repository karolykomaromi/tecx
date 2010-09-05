using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

using TecX.Common;

namespace TecX.Unity
{
    public class ClozeInjectionConstructur : InjectionMember
    {
        #region Fields

        private readonly IDictionary<string, object> _ctorArguments;

        #endregion Fields

        #region c'tor

        private ClozeInjectionConstructur()
        {
            _ctorArguments = new Dictionary<string, object>();
        }

        public ClozeInjectionConstructur(string argName, object argValue)
            : this()
        {
            Guard.AssertNotEmpty(argName, "argName");
            Guard.AssertNotNull(argValue, "argValue");

            _ctorArguments.Add(argName, argValue);
        }

        public ClozeInjectionConstructur(IEnumerable<KeyValuePair<string, object>> ctorArguments)
            : this()
        {
            Guard.AssertNotNull(ctorArguments, "ctorArguments");

            foreach (var ctorArgument in ctorArguments)
            {
                _ctorArguments.Add(ctorArgument);
            }
        }

        #endregion c'tor

        #region Methods

        public override void AddPolicies(Type serviceType, Type implementationType, string name,
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

            //fill in the provided values and add the type of the parameter that the
            //ctor expects otherwise. unity will take care of resolving parameters that
            //are provided that way
            for (int i = 0; i < infos.Length; i++)
            {
                object value;
                if (_ctorArguments.TryGetValue(infos[i].Name, out value))
                {
                    parameterValues[i] = value;
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

            ParameterMatcher matcher = new ParameterMatcher(_ctorArguments);

            return matcher.BestMatch(ctors);
        }

        #endregion Methods
    }
}