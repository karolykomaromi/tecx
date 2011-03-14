using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration.Lifetime
{
    public class Singleton : Lifetime
    {
        public override LifetimeManager Build()
        {
            return new ContainerControlledLifetimeManager();
        }
    }
}