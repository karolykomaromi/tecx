namespace Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
    using Microsoft.Practices.Prism.Logging;

    public class CacheBackedResourceManager : IResourceManager
    {
        private readonly ObjectCache cache;
        private readonly ILoggerFacade logger;

        public CacheBackedResourceManager(ObjectCache cache, ILoggerFacade logger)
        {
            Contract.Requires(cache != null);
            Contract.Requires(logger != null);

            this.cache = cache;
            this.logger = logger;
        }

        public string GetString(string name, CultureInfo culture)
        {
            string cached = this.cache[name] as string;

            if (string.IsNullOrEmpty(cached))
            {
                this.logger.Log(string.Format(CultureInfo.CurrentCulture, "Cache miss. Name=\"{0}\"", name), Category.Debug, Priority.Low);
                return (name + "_" + culture.TwoLetterISOLanguageName).ToUpperInvariant();
            }

            string msg = string.Format(CultureInfo.CurrentCulture, "Cache hit. Name=\"{0}\" Value=\"{1}\"", name, cached);
            this.logger.Log(msg, Category.Debug, Priority.Low);
            return cached;
        }
    }
}
