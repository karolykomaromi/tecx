namespace Hydra.JobHost.Configuration
{
    using Common.Logging;
    using Microsoft.Practices.Unity;

    public class LoggingConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ILog>(new InjectionFactory((c, t, n) => LogManager.GetLogger(t)));
        }
    }
}
