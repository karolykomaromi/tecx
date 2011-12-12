namespace TecX.Unity.Configuration
{
    using Microsoft.Practices.Unity;

    public interface IConfigAction
    {
        void Configure(IUnityContainer container);
    }
}
