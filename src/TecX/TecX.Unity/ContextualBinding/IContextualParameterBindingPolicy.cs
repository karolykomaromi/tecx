namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    public interface IContextualParameterBindingPolicy : IBuilderPolicy
    {
        void Add(ContextualResolverOverride contextualResolverOverride);

        void SetResolverOverrides(IRequest request);
    }
}