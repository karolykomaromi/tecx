namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Runtime.Caching;
    using Hydra.Infrastructure.Caching;

    public class CachingSettingsProvider : ISettingsProvider, IDisposable
    {
        /// <summary>
        /// Settings
        /// </summary>
        private const string SettingsCacheKey = "Settings";

        private readonly ObjectCache cache;
        private readonly ISettingsProvider readThrough;

        private IDisposable invalidationToken;

        public CachingSettingsProvider(ObjectCache cache, ISettingsProvider readThrough)
        {
            this.cache = cache;
            this.readThrough = readThrough;
        }

        public SettingsCollection GetSettings()
        {
            SettingsCollection settings = this.cache.Get(SettingsCacheKey) as SettingsCollection;

            if (settings == null)
            {
                settings = this.readThrough.GetSettings();

                this.cache.Add(SettingsCacheKey, settings);
            }

            return settings;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.invalidationToken != null)
                {
                    this.invalidationToken.Dispose();
                    this.invalidationToken = null;
                }
            }
        }
    }
}