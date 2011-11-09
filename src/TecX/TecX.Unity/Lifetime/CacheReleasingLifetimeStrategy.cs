namespace TecX.Unity.Lifetime
{
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class CacheReleasingLifetimeStrategy : BuilderStrategy
    {
        public override void PostTearDown(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            var lifetimes = context.Lifetime.OfType<CacheLifetimeManager>();

            foreach (var lifetimePolicy in lifetimes)
            {
                lifetimePolicy.RemoveValue();
            }
        }
    } 
}
