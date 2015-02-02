namespace TecX.Unity.Configuration
{
    using Microsoft.Practices.Unity;

    public interface IConfigure
    {
        void Configure(IUnityContainer container);
    }
}
