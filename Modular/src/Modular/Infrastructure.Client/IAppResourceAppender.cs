namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using Infrastructure.I18n;

    [ContractClass(typeof(AppResourceAppenderContract))]
    public interface IAppResourceAppender
    {
        void Add(ResourceDictionary resourceDictionary);

        void Add(IResourceManager resourceManager);
    }

    [ContractClassFor(typeof(IAppResourceAppender))]
    public abstract class AppResourceAppenderContract : IAppResourceAppender
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
