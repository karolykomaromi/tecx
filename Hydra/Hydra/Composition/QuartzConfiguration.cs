namespace Hydra.Composition
{
    using Hydra.Jobs.Client;
    using Microsoft.Practices.Unity;

    public class QuartzConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ISchedulerClient, NullSchedulerClient>();
        }
    }
}