namespace TecX.Unity.Literals
{
    using Microsoft.Practices.ObjectBuilder2;

    public interface IDependencyResolverConvention
    {
        bool CanCreateResolver(IBuilderContext context, DependencyInfo dependency);

        IDependencyResolverPolicy CreateResolver(IBuilderContext context, DependencyInfo dependency);
    }
}