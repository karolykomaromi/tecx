namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Caching;
    using Hydra.Infrastructure.Logging;
    using Hydra.Infrastructure.Reflection;

    [DebuggerDisplay("Count={cacheItemInvalidationTokens.Count}")]
    public class ResourceAccessorCache : IDisposable, IResourceAccessorCache
    {
        public static readonly Func<string> EmptyAccessor = () => string.Empty;
        
        private readonly IResxPropertyConvention convention;
        private readonly ObjectCache cache;
        private readonly List<IDisposable> cacheItemInvalidationTokens;

        public ResourceAccessorCache(IResxPropertyConvention convention, ObjectCache cache)
        {
            Contract.Requires(convention != null);
            Contract.Requires(cache != null);

            this.cacheItemInvalidationTokens = new List<IDisposable>();
            this.convention = convention;
            this.cache = cache;
        }

        public Func<string> GetAccessor(Type modelType, string propertyName)
        {
            string accessorName = modelType.FullName.Replace('.', '_') + "_" + propertyName;

            Func<string> accessor;
            if ((accessor = this.cache[accessorName] as Func<string>) == null)
            {
                Assembly assembly = modelType.Assembly;

                string resourceTypeName = assembly.GetName().Name + ".Properties.Resources";

                Type resourceType = assembly.GetType(resourceTypeName, false);

                if (resourceType == null)
                {
                    accessor = ResourceAccessorCache.EmptyAccessor;
                    this.cacheItemInvalidationTokens.Add(this.cache.Add(accessorName, accessor));
                    HydraEventSource.Log.ResourceTypeNotFound(assembly, resourceTypeName);
                    return accessor;
                }

                PropertyInfo property = this.convention.FindProperty(resourceType, modelType, propertyName);

                if (property == Property.Null)
                {
                    accessor = ResourceAccessorCache.EmptyAccessor;
                    this.cacheItemInvalidationTokens.Add(this.cache.Add(accessorName, accessor));
                    HydraEventSource.Log.ResourcePropertyNotFound(resourceType, propertyName);
                    return accessor;
                }

                MemberExpression expression = Expression.Property(null, property);
                Expression<Func<string>> lambda = Expression.Lambda<Func<string>>(expression);
                accessor = lambda.Compile();
                this.cacheItemInvalidationTokens.Add(this.cache.Add(accessorName, accessor));
            }

            return accessor;
        }

        public void Clear()
        {
            foreach (IDisposable token in this.cacheItemInvalidationTokens)
            {
                try
                {
                    token.Dispose();
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Warning(ex);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Clear();
            }
        }
    }
}
