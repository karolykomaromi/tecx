namespace Hydra.Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Caching;

    public class CachingSupportedCulturesProvider : SupportedCulturesProvider, IDisposable
    {
        /// <summary>
        /// SupportedCultures
        /// </summary>
        private const string SupportedCulturesCacheKey = "SupportedCultures";

        private readonly SupportedCulturesProvider readThrough;
        private readonly ObjectCache cache;

        private IDisposable invalidationToken;

        public CachingSupportedCulturesProvider(ObjectCache cache, SupportedCulturesProvider readThrough)
        {
            Contract.Requires(readThrough != null);
            Contract.Requires(cache != null);

            this.readThrough = readThrough;
            this.cache = cache;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected internal override CultureInfo[] GetSupportedCultures()
        {
            CultureInfo[] supportedCultures = this.cache.Get(SupportedCulturesCacheKey) as CultureInfo[];

            if (supportedCultures == null)
            {
                supportedCultures = this.readThrough.GetSupportedCultures();

                this.invalidationToken = this.cache.Add(SupportedCulturesCacheKey, supportedCultures);
            }

            return supportedCultures;
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this.invalidationToken != null)
            {
                this.invalidationToken.Dispose();
                this.invalidationToken = null;
            }
        }
    }
}