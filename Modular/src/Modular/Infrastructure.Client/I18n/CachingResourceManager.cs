namespace Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows.Threading;

    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    public class CachingResourceManager : IResourceManager
    {
        private readonly IResourceManager inner;
        private readonly ICacheInvalidationManager cacheInvalidationManager;
        private readonly ObjectCache cache;

        public CachingResourceManager(IResourceManager inner, ICacheInvalidationManager cacheInvalidationManager, ObjectCache cache)
        {
            Contract.Requires(inner != null);
            Contract.Requires(cacheInvalidationManager != null);
            Contract.Requires(cache != null);

            this.inner = inner;
            this.cacheInvalidationManager = cacheInvalidationManager;
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
                    ExternalInvalidationPolicy policy = new ExternalInvalidationPolicy { SlidingExpiration = TimeSpan.FromMinutes(10) };

                    this.cacheInvalidationManager.Invalidated += policy.OnInvalidated;

                    this.cache.Add(key, value, policy);

                    return value;
                }

                return key;
            }
        }
    }

    public interface ICacheInvalidationManager
    {
        event EventHandler Invalidated;

        void RequestInvalidate();
    }

    public class ExternalInvalidationPolicy : CacheItemPolicy
    {
        public void OnInvalidated(object sender, EventArgs args)
        {
            this.AbsoluteExpiration = TimeProvider.Now;
        }
    }

    public class CacheInvalidationManager : ICacheInvalidationManager
    {
        private readonly List<WeakReference> eventHandlers;
        private readonly Dispatcher dispatcher;

        private bool hasCanExecuteQueued;

        public CacheInvalidationManager(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
            this.eventHandlers = new List<WeakReference>();
        }

        public event EventHandler Invalidated
        {
            add { this.AddWeakReferenceHandler(value); }

            remove { this.RemoveWeakReferenceHandler(value); }
        }

        public void RequestInvalidate()
        {
            if (this.hasCanExecuteQueued)
            {
                return;
            }

            this.hasCanExecuteQueued = true;

            this.dispatcher.BeginInvoke(
                () =>
                {
                    this.CallWeakReferenceHandlers();
                    this.hasCanExecuteQueued = false;
                });
        }

        private void RemoveWeakReferenceHandler(EventHandler handler)
        {
            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];
                EventHandler target = reference.Target as EventHandler;
                if (target == null || 
                    target == handler)
                {
                    this.eventHandlers.RemoveAt(i);
                }
            }
        }

        private void AddWeakReferenceHandler(EventHandler handler)
        {
            this.eventHandlers.Add(new WeakReference(handler));
        }

        private void CallWeakReferenceHandlers()
        {
            EventHandler[] handlerArray = new EventHandler[this.eventHandlers.Count];
            int index = 0;
            for (int i = this.eventHandlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.eventHandlers[i];
                EventHandler target = reference.Target as EventHandler;
                if (target == null)
                {
                    this.eventHandlers.RemoveAt(i);
                }
                else
                {
                    handlerArray[index] = target;
                    index++;
                }
            }

            for (int j = 0; j < index; j++)
            {
                EventHandler handler2 = handlerArray[j];
                handler2(this, EventArgs.Empty);
            }
        }
    }
}

//<?xml version="1.0" encoding="utf-8"?>
//<el:ConfigurationDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
//    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" 
//    xmlns:el="http://schemas.microsoft.com/practices/2011/entlib">
//    <el:CachingSettings DefaultCache="In-Memory Cache" x:Key="cachingSilverlightConfiguration">
//        <el:CachingSettings.Caches>
//            <el:InMemoryCacheData ExpirationPollingInterval="00:02:00" Name="In-Memory Cache" />
//            <el:IsolatedStorageCacheData 
//                MaxSizeInKilobytes="5120" 
//                PercentOfQuotaUsedBeforeScavenging="50" 
//                PercentOfQuotaUsedAfterScavenging="20" 
//                ExpirationPollingInterval="00:01:00" 
//                Name="Isolated Storage Cache" />
//        </el:CachingSettings.Caches>
//    </el:CachingSettings>
//</el:ConfigurationDictionary>

//string xaml;
//using (Stream s = this.GetType().Assembly.GetManifestResourceStream("SilverlightCaching.CacheConfig.xaml"))
//{
//    using (StreamReader sr = new StreamReader(s))
//    {
//        xaml = sr.ReadToEnd();
//    }
//}

//var configDictionary = (IDictionary)XamlReader.Load(xaml);
//var configSource = DictionaryConfigurationSource.FromDictionary(configDictionary);

//IUnityContainer container = new UnityContainer();

//EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(container), configSource);

//container.AddNewExtension<ViewModelLocatorExtension>();

//EnterpriseLibraryContainer.Current = new UnityServiceLocator(container);

//this.RootVisual = EnterpriseLibraryContainer.Current.GetInstance<MainPageView>();

//    public class ExternalInvalidationPolicy : CacheItemPolicy, ISubscribeTo<InvalidateCache>
//    {
//        public void Handle(InvalidateCache message)
//        {
//            this.AbsoluteExpiration = DateTime.Now;
//        }
//    }