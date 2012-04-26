namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    public abstract class ContextualResolverOverride
    {
        public abstract bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext);

        public abstract void SetResolverOverrides(IBuilderContext context);
    }
}