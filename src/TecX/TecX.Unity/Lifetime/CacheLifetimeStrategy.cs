namespace TecX.Unity.Lifetime
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class CacheLifetimeStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            IPolicyList policySource;
            var lifetimePolicy = context
                .PersistentPolicies
                .Get<ILifetimePolicy>(context.BuildKey, out policySource);

            if (object.ReferenceEquals(policySource, context.PersistentPolicies))
            {
                return;
            }

            var cacheLifetime = lifetimePolicy as CacheLifetimeManager;

            if (cacheLifetime == null)
            {
                return;
            }

            var childLifetime = cacheLifetime.Clone();

            context
                .PersistentPolicies
                .Set<ILifetimePolicy>(childLifetime, context.BuildKey);

            context.Lifetime.Add(childLifetime);
        }
    } 
}
