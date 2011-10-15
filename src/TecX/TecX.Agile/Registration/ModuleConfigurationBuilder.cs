using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Agile.Infrastructure;
using TecX.Agile.Modules.Main;
using TecX.Unity.Collections;
using TecX.Unity.Configuration;
using TecX.Unity.Enrichment;
using TecX.Unity.TypedFactory;

namespace TecX.Agile.Registration
{
    public class ModuleConfigurationBuilder : ConfigurationBuilder
    {
        public ModuleConfigurationBuilder()
        {
            AddExpression(config => config.AddModification(container => container.AddNewExtension<TypedFactoryExtension>()));
            AddExpression(config => config.AddModification(container => container.AddNewExtension<EnrichmentExtension>()));
            AddExpression(config => config.AddModification(container => container.AddNewExtension<CollectionResolutionExtension>()));

            For<IModule>().Add<Module>().Named("Main")
                .EnrichWith<Module>((module, ctx) => module.Initialize());
        }
    }
}
