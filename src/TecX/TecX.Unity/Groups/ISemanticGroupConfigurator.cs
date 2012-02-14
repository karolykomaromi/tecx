namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    public interface ISemanticGroupConfigurator : IUnityContainerExtensionConfigurator
    {
        ISemanticGroup RegisterAsGroup(Type from, Type to, string name);
    }
}