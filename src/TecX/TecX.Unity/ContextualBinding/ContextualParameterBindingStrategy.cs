namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Unity.Tracking;

    public class ContextualParameterBindingStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = context.Policies.Get<IContextualParameterBindingPolicy>(context.BuildKey);

            if (policy != null)
            {
                policy.SetResolverOverrides(Request.Current);
            }
        }
    }
}