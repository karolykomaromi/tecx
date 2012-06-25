namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    public interface ISemanticGroupConfigurator : IUnityContainerExtensionConfigurator
    {
        void AddPolicy(ISemanticGroupPolicy policy, Type type, string name);
    }
}