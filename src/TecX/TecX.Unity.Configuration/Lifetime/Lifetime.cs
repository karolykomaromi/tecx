using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Lifetime
{
    public abstract class Lifetime
    {
        public abstract LifetimeManager Build();

        public static implicit operator LifetimeManager(Lifetime instance)
        {
            Guard.AssertNotNull(instance, "instance");

            return instance.Build();
        }
    }
}