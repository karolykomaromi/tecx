namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    public class ContextualParameterBindingStrategy : BuilderStrategy
    {
        private readonly IBindingContext bindingContext;

        public ContextualParameterBindingStrategy(IBindingContext bindingContext)
        {
            this.bindingContext = bindingContext;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = context.Policies.Get<IContextualParameterBindingPolicy>(context.BuildKey);

            if (policy != null)
            {
                if (policy.IsMatch(this.bindingContext, context))
                {
                    policy.SetResolverOverrides(context);
                }
            }
        }
    }
}