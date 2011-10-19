using System.Collections.Generic;

using Caliburn.Micro;

using Microsoft.Practices.Unity;

using TecX.Agile.Infrastructure;
using TecX.Agile.ViewModels;
using TecX.Unity.Configuration;

namespace TecX.Agile
{
    public class AppBootstrapper : UnityBootstrapper<ShellViewModel>
    {
        public AppBootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        protected override void Configure()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.Scan(x =>
                {
                    x.LookForConfigBuilders();
                    x.AssembliesFromApplicationBaseDirectory();
                });

            Container.AddExtension(builder);
        }
    }
}
