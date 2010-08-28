using Microsoft.Practices.Unity;

namespace TecX.Common.Event.Unity
{
    public interface IEventAggregatorConfiguration : IUnityContainerExtensionConfigurator
    {
        IEventAggregator EventAggregator { get; }
    }
}