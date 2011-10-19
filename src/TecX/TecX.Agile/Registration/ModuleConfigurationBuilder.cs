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
            For<IModule>().Add<Module>().Named("Main")
                .EnrichWith<IModule>((module, ctx) => module.Initialize());

            For<IModule>().Add<Modules.Gestures.Module>().Named("Gestures")
                .EnrichWith<IModule>((module, ctx) => module.Initialize());
        }
    }
}
