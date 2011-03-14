using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration.Lifetime
{
    public class PerCall : Lifetime
    {
        public override LifetimeManager Build()
        {
            return new TransientLifetimeManager();
        }
    }
}
