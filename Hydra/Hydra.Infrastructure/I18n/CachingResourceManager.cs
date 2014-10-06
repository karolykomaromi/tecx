namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Caching;

    [DebuggerDisplay("BaseName={BaseName}")]
    public class CachingResourceManager : ResourceManagerBase, IDisposable
    {
        private readonly List<IDisposable> cacheItemInvalidationTokens;
        private readonly ObjectCache cache;
        private readonly IResourceManager readThrough;
        private bool isDisposed;

        /// <summary>
        /// Creates a new instance of the <see cref="CachingResourceManager"/> class that acts as a purely cache backed resource manager.
        /// Any resource that is not found in the cache will be transformed to the default placeholder.
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="cache"></param>
        public CachingResourceManager(string baseName, ObjectCache cache)
            : this(baseName, cache, ResourceManagerBase.Null(baseName))
        {
        }

        public CachingResourceManager(string baseName, ObjectCache cache, IResourceManager readThrough)
            : base(baseName)
        {
            Contract.Requires(cache != null);
            Contract.Requires(readThrough != null);

            this.cacheItemInvalidationTokens = new List<IDisposable>();
            this.cache = cache;
            this.readThrough = readThrough;
        }

        public ObjectCache Cache
        {
            get { return this.cache; }
        }

        public IResourceManager ReadThrough
        {
            get { return this.readThrough; }
        }

        public override string GetString(string name, CultureInfo culture)
        {
            string key = this.GetKey(name, culture);

            string s = this.Cache[key] as string;

            if (s != null)
            {
                return s;
            }

            s = this.ReadThrough.GetString(name, culture);

            IDisposable token = this.Cache.Add(key, s);

            this.cacheItemInvalidationTokens.Add(token);

            return s;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.isDisposed)
            {
                foreach (IDisposable token in this.cacheItemInvalidationTokens)
                {
                    try
                    {
                        token.Dispose();
                    }
                    catch
                    {
                        // I prefer to die silently
                    }
                }

                this.isDisposed = true;
            }
        }
    }
}