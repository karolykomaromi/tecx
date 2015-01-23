namespace Hydra.Infrastructure.Configuration
{
    public class InMemorySettingsProvider : ISettingsProvider
    {
        private readonly SettingsCollection settings;

        public InMemorySettingsProvider(params Setting[] settings)
        {
            this.settings = new SettingsCollection(settings);
        }

        public SettingsCollection GetSettings()
        {
            return this.settings;
        }
    }
}