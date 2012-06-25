namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    public interface IGroupedMappings : IUnityContainerExtensionConfigurator
    {
        void AddPolicy(IMappingGroupPolicy policy, Type type, string name);
    }
}