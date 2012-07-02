namespace TecX.Unity.Literals
{
    using Microsoft.Practices.Unity;

    public interface ILiteralParameters : IUnityContainerExtensionConfigurator
    {
        void AddConvention(IDependencyResolverConvention convention);
    }
}