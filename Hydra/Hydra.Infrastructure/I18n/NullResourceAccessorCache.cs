namespace Hydra.Infrastructure.I18n
{
    using System;

    public class NullResourceAccessorCache : IResourceAccessorCache
    {
        public Func<string> GetAccessor(Type modelType, string propertyName)
        {
            return () => StringHelper.SplitCamelCase(propertyName);
        }

        public void Clear()
        {
        }
    }
}