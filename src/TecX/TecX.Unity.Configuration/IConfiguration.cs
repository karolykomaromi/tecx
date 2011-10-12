using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration
{
    public interface IConfiguration
    {
        void Configure(IUnityContainer container);
    }
}
