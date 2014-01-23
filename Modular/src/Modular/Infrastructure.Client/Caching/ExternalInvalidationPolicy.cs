using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Infrastructure.Caching
{
    public class ExternalInvalidationPolicy : CacheItemPolicy
    {
        public void OnInvalidated(object sender, EventArgs args)
        {
            this.AbsoluteExpiration = TimeProvider.Now;
        }
    }
}