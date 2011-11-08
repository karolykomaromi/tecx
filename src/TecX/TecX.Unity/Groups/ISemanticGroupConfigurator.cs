namespace TecX.Unity.Groups
{
    using Microsoft.Practices.Unity;

    public interface ISemanticGroupConfigurator : IUnityContainerExtensionConfigurator
    {
        ISemanticGroup RegisterGroup<TFrom, TTo>(string name);
    }
}