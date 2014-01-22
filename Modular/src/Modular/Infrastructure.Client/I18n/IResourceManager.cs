namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

    [ContractClass(typeof(ResourceManagerContract))]
    public interface IResourceManager
    {
        string this[string key] { get; }
    }

    [ContractClassFor(typeof(IResourceManager))]
    public abstract class ResourceManagerContract : IResourceManager
    {
        public string this[string key]
        {
            get
            {
                Contract.Requires(!string.IsNullOrEmpty(key));
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                return " ";
            }
        }
    }

    public class ResxFilesResourceManager : IResourceManager
    {
        public string this[string key]
        {
            get
            {
                Assembly callingAssembly = Assembly.GetCallingAssembly();

                return key;
            }
        }
    }

    public class WebSvcResourceManager : IResourceManager
    {
        public string this[string key]
        {
            get { return key; }
        }
    }

    public class EchoResourceManager : IResourceManager
    {
        public string this[string key]
        {
            get { return key; }
        }
    }

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
                object cached = this.cache[key];

                if (cached != null)
                {
                    return (string)cached;
                }

                string resource = this.inner[key];

                this.cache.Add(key, resource, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(15) });

                return resource;
            }
        }
    }
}
