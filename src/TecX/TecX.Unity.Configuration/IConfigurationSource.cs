using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration
{
    public interface IConfigurationSource
    {
        void Configure(IUnityContainer container);
    }
}
