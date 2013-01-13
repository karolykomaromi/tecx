namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Unity.Tracking;

    public abstract class ContextualResolverOverride
    {
        public abstract bool IsMatch(IRequest request);

        public abstract void SetResolverOverrides(IBuilderContext context);
    }
}