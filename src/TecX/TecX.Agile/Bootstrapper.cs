using Caliburn.Micro;

using TecX.Unity.Configuration;

namespace TecX.Agile
{
    public class Bootstrapper : UnityBootstrapper<ShellViewModel>
    {
        public Bootstrapper()
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
