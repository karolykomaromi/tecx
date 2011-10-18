using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Unity.Configuration;

namespace TecX.Agile
{
    using TecX.Agile.ViewModels;

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
