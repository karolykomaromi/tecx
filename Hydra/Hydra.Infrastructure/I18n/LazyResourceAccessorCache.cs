namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;

    public class LazyResourceAccessorCache : IResourceAccessorCache
    {
        private readonly Lazy<IResourceAccessorCache> instance;

        public LazyResourceAccessorCache(Func<IResourceAccessorCache> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IResourceAccessorCache>(factory);
        }

        public Func<string> GetAccessor(Type modelType, string propertyName)
        {
            return this.instance.Value.GetAccessor(modelType, propertyName);
        }

        public void Clear()
        {
            this.instance.Value.Clear();
        }
    }
}