namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Unity.Tracking;

    public interface IContextualParameterBindingPolicy : IBuilderPolicy
    {
        void Add(ContextualResolverOverride contextualResolverOverride);

        void SetResolverOverrides(IRequest request);
    }
}