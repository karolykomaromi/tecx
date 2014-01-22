namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using Infrastructure.I18n;

    [ContractClass(typeof(ApplicationResourcesContract))]
    public interface IApplicationResources
    {
        void Add(ResourceDictionary resourceDictionary);

        void Add(IResourceManager resourceManager);
    }

    [ContractClassFor(typeof(IApplicationResources))]
    public abstract class ApplicationResourcesContract : IApplicationResources
    {
        public void Add(ResourceDictionary resourceDictionary)
        {
            Contract.Requires(resourceDictionary != null);
        }

        public void Add(IResourceManager resourceManager)
        {
            Contract.Requires(resourceManager != null);
        }
    }
}
