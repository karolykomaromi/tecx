namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    public abstract class ContextualResolverOverride
    {
        public abstract bool IsMatch(IRequest request);

        public abstract void SetResolverOverrides(IBuilderContext context);
    }
}