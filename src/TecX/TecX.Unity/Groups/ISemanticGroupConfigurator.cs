namespace TecX.Unity.Groups
{
    using Microsoft.Practices.Unity;

    public interface ISemanticGroupConfigurator : IUnityContainerExtensionConfigurator
    {
        ISemanticGroup RegisterAsGroup<TFrom, TTo>(string name);
    }
}