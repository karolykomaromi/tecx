using System;
using System.Collections.Generic;
using System.Linq;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class SingleImplementationOfInterfaceConvention : IRegistrationConvention
    {
        private readonly Cache<Type, List<Type>> _types;

        private readonly Action<ConfigurationBuilder> _hookUp;

        public SingleImplementationOfInterfaceConvention()
        {
            _types = new Cache<Type, List<Type>>(t => new List<Type>());
            _hookUp = new RunOnce<ConfigurationBuilder>(builder => builder.AddExpression(Modify));
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            RegisterType(type);

            _hookUp(builder);
        }

        public void Modify(Configuration graph)
        {
            Guard.AssertNotNull(graph, "graph");

            ConfigurationBuilder singleImplementationBuilder = new ConfigurationBuilder();

            _types.Each((pluginType, types) =>
            {
                if (types.Count == 1)
                {
                    singleImplementationBuilder.For(pluginType).Add(types[0]).Named(types[0].FullName);
                }
            });

            singleImplementationBuilder.BuildUp(graph);
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
