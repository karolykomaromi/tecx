namespace Hydra.Infrastructure.Configuration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class SettingsCollection : IEnumerable<Setting>
    {
        private readonly Dictionary<SettingName, Setting> settings;

        public SettingsCollection(params Setting[] settings)
        {
            this.settings = (settings ?? new Setting[0]).ToDictionary(s => s.Name);
        }

        public Setting this[SettingName name]
        {
            get
            {
                Contract.Requires(name != null);
                Contract.Ensures(Contract.Result<Setting>() != null);

                Setting setting;
                if (this.settings.TryGetValue(name, out setting))
                {
                    return setting;
                }

                return Setting.Empty;
            }
        }

        public void Add(Setting setting)
        {
            Contract.Requires(setting != null);

            this.settings[setting.Name] = setting;
        }

        public IEnumerator<Setting> GetEnumerator()
        {
            return this.settings.Values.OrderBy(s => s.Name, Comparer<SettingName>.Default).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}