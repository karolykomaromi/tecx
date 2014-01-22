namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    public class CachingResourceManager : IResourceManager
    {
        private readonly IResourceManager inner;
        private readonly ObjectCache cache;

        public CachingResourceManager(IResourceManager inner, ObjectCache cache)
        {
            Contract.Requires(inner != null);
            Contract.Requires(cache != null);

            this.inner = inner;
            this.cache = cache;
        }

        public string this[string key]
        {
            get
            {
                string cached = this.cache[key] as string;

                if (cached != null)
                {
                    return (string)cached;
                }

                string value = this.inner[key];

                if (!string.Equals(key, value, StringComparison.Ordinal))
                {
                    this.cache.Add(key, value, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(15) });

                    return value;
                }

                return key;
            }
        }
    }

    /*

<?xml version="1.0" encoding="utf-8"?>
<el:ConfigurationDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" 
    xmlns:el="http://schemas.microsoft.com/practices/2011/entlib">
    <el:CachingSettings DefaultCache="In-Memory Cache" x:Key="cachingSilverlightConfiguration">
        <el:CachingSettings.Caches>
            <el:InMemoryCacheData ExpirationPollingInterval="00:02:00" Name="In-Memory Cache" />
            <el:IsolatedStorageCacheData 
                MaxSizeInKilobytes="5120" 
                PercentOfQuotaUsedBeforeScavenging="50" 
                PercentOfQuotaUsedAfterScavenging="20" 
                ExpirationPollingInterval="00:01:00" 
                Name="Isolated Storage Cache" />
        </el:CachingSettings.Caches>
    </el:CachingSettings>
</el:ConfigurationDictionary>

string xaml;
using (Stream s = this.GetType().Assembly.GetManifestResourceStream("SilverlightCaching.CacheConfig.xaml"))
{
	using (StreamReader sr = new StreamReader(s))
	{
		xaml = sr.ReadToEnd();
	}
}

var configDictionary = (IDictionary)XamlReader.Load(xaml);
var configSource = DictionaryConfigurationSource.FromDictionary(configDictionary);

IUnityContainer container = new UnityContainer();

EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(container), configSource);

container.AddNewExtension<ViewModelLocatorExtension>();

EnterpriseLibraryContainer.Current = new UnityServiceLocator(container);

this.RootVisual = EnterpriseLibraryContainer.Current.GetInstance<MainPageView>();

    public class ExternalInvalidationPolicy : CacheItemPolicy, ISubscribeTo<InvalidateCache>
    {
        public void Handle(InvalidateCache message)
        {
            this.AbsoluteExpiration = DateTime.Now;
        }
    }

     */
}