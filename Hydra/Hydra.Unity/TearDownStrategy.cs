namespace Hydra.Unity
{
    using System.Linq;
    using Microsoft.Practices.ObjectBuilder2;

    public class TearDownStrategy : BuilderStrategy
    {
        public override void PostTearDown(IBuilderContext context)
        {
            var lifetimePolicies = context.Lifetime.OfType<ILifetimePolicy>();

            foreach (ILifetimePolicy policy in lifetimePolicies)
            {
                policy.RemoveValue();
            }
        }
    }
}