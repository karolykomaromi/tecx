using System;
using System.Collections.Generic;
using System.Linq;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class SingleImplementationOfInterfaceConvention : IRegistrationConvention, 
        INeedPostScanningAction
    {
        #region Fields

        private readonly Cache<Type, List<Type>> _types;

        #endregion Fields

        #region c'tor

        public SingleImplementationOfInterfaceConvention()
        {
            _types = new Cache<Type, List<Type>>(t => new List<Type>());
        }

        #endregion c'tor

        public void Process(Type type, Registry registry)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(registry, "registry");

            RegisterType(type);
        }

        public void PostScanningAction(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            Registry singleImplementationRegistry = new Registry();

            _types.Each((pluginType, types) =>
            {
                if (types.Count == 1)
                {
                    singleImplementationRegistry.AddType(pluginType, types[0], types[0].FullName);
                }
            });

            singleImplementationRegistry.ConfigureRegistrationGraph(graph);
        }

        private void Register(Type interfaceType, Type concreteType)
        {
            _types[interfaceType].Add(concreteType);
        }

        private void RegisterType(Type type)
        {
            if (!type.CanBeCreated())
            {
                return;
            }

            type.GetInterfaces()
                .Where(i => i.IsVisible)
                .Each(i => Register(i, type));
        }
    }
}
