using Microsoft.Practices.Unity;

namespace TecX.Unity.Test.TestObjects
{
    public interface ITestExtensionConfig : IUnityContainerExtensionConfigurator
    {
        bool Prop1 { get; set; }
    }
}