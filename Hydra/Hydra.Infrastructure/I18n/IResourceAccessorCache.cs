namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ResourceAccessorCacheContract))]
    public interface IResourceAccessorCache
    {
        Func<string> GetAccessor(Type modelType, string propertyName);
        
        void Clear();
    }

    [ContractClassFor(typeof(IResourceAccessorCache))]
    internal abstract class ResourceAccessorCacheContract : IResourceAccessorCache
    {
        public Func<string> GetAccessor(Type modelType, string propertyName)
        {
            Contract.Requires(modelType != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            Contract.Ensures(Contract.Result<Func<string>>() != null);

            return () => string.Empty;
        }

        public void Clear()
        {
        }
    }
}