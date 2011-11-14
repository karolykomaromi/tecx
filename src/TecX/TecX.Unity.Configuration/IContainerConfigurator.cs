namespace TecX.Unity.Configuration
{
    using Microsoft.Practices.Unity;

    public interface IContainerConfigurator
    {
        void Configure(IUnityContainer container);
    }
}
