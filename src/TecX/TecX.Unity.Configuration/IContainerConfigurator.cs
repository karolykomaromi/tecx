using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration
{
    public interface IContainerConfigurator
    {
        void Configure(IUnityContainer container);
    }
}
