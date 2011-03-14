using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration.Lifetime
{
    public class PerThread : Lifetime
    {
        public override LifetimeManager Build()
        {
            return new PerThreadLifetimeManager();
        }
    }
}
