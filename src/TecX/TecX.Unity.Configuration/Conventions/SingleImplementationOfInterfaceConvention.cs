using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class SingleImplementationOfInterfaceConvention : IRegistrationConvention
    {
        private readonly Cache<Type, List<Type>> _types;

        public SingleImplementationOfInterfaceConvention()
        {
            _types = new Cache<Type, List<Type>>(t => new List<Type>());
        }

        public void Process(Type type, Registry registry)
        {
            RegisterType(type);
        }

        private void Register(Type interfaceType, Type concreteType)
        {
            _types[interfaceType].Add(concreteType);
        }

        private void RegisterType(Type type)
        {
            if (!type.CanBeCreated()) return;

            type.GetInterfaces().Where(i => i.IsVisible).Each(i => Register(i, type));
        }

        public void RegisterSingleImplementations(RegistrationGraph graph)
        {
            var singleImplementationRegistry = new SingleImplementationRegistry();

            _types.Each((pluginType, types) =>
            {
                if (types.Count == 1)
                {
                    singleImplementationRegistry.AddType(pluginType, types[0], types[0].FullName);
                    //ConfigureFamily(singleImplementationRegistry.For(pluginType));
                }
            });

            singleImplementationRegistry.ConfigureRegistrationGraph(graph);
        }

        private class SingleImplementationRegistry : Registry
        {
            // This type created just to make the output clearer in WhatDoIHave()
            // might consider adding a Description property to Registry instead
        }
    }
}
