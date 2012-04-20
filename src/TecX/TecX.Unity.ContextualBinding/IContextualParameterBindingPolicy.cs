namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    public interface IContextualParameterBindingPolicy : IBuilderPolicy
    {
        bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext);

        void SetResolverOverrides(IBuilderContext context);
    }
}