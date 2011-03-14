using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration.Lifetime
{
    public class ExternallyControlled : Lifetime
    {
        public override LifetimeManager Build()
        {
            return new ExternallyControlledLifetimeManager();
        }
    }
}
