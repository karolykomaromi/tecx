namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    public class ContextualParameterBindingStrategy : BuilderStrategy
    {
        private readonly IRequest request;

        public ContextualParameterBindingStrategy(IRequest request)
        {
            this.request = request;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = context.Policies.Get<IContextualParameterBindingPolicy>(context.BuildKey);

            if (policy != null)
            {
                policy.SetResolverOverrides(this.request, context);
            }
        }
    }
}