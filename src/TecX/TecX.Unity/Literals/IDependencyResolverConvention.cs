namespace TecX.Unity.Literals
{
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IDependencyResolverConvention
    {
        bool CanCreateResolver(IBuilderContext context, ParameterInfo parameter);

        IDependencyResolverPolicy CreateResolver(IBuilderContext context, ParameterInfo parameter);
    }
}