namespace TecX.Unity.Configuration.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;
    using TecX.Unity.Configuration.Common;
    using TecX.Unity.Configuration.Extensions;

    public class SingleImplementationOfInterfaceConvention : IRegistrationConvention
    {
        private readonly Cache<Type, List<Type>> types;

        private readonly Action<ConfigurationBuilder> hookUp;

        public SingleImplementationOfInterfaceConvention()
        {
            this.types = new Cache<Type, List<Type>>(t => new List<Type>());
            this.hookUp = new RunOnce<ConfigurationBuilder>(builder => builder.AddExpression(this.Modify));
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            this.RegisterType(type);

            this.hookUp(builder);
        }

        public void Modify(Configuration graph)
        {
            Guard.AssertNotNull(graph, "graph");

            ConfigurationBuilder singleImplementationBuilder = new ConfigurationBuilder();

            this.types.Each((pluginType, typeList) =>
            {
                if (typeList.Count == 1)
                {
                    singleImplementationBuilder.For(pluginType).Add(typeList[0]).Named(typeList[0].FullName);
                }
            });

            singleImplementationBuilder.BuildUp(graph);
        }

        private void Register(Type interfaceType, Type concreteType)
        {
            this.types[interfaceType].Add(concreteType);
        }

        private void RegisterType(Type type)
        {
            if (!type.CanBeCreated())
            {
                return;
            }

            type.GetInterfaces()
                .Where(i => i.IsVisible)
                .Each(i => this.Register(i, type));
        }
    }
}
