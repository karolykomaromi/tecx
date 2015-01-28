namespace Hydra.Infrastructure.Configuration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class SettingsCollection : IEnumerable<Setting>, IFreezable<SettingsCollection>
    {
        private readonly Dictionary<SettingName, Setting> settings;

        public SettingsCollection(params Setting[] settings)
        {
            this.settings = (settings ?? new Setting[0]).ToDictionary(s => s.Name);
        }

        public int Count
        {
            get { return this.settings.Count; }
        }

        public virtual bool IsMutable
        {
            get { return true; }
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

        public virtual void Add(Setting setting)
        {
            Contract.Requires(setting != null);

            this.settings[setting.Name] = setting;
        }

        public virtual SettingsCollection Merge(SettingsCollection winner)
        {
            Contract.Requires(winner != null);
            Contract.Ensures(Contract.Result<SettingsCollection>() != null);

            SettingsCollection merged = new SettingsCollection();

            foreach (Setting setting in this)
            {
                merged.Add(setting);
            }

            foreach (Setting setting in winner)
            {
                merged.Add(setting);
            }

            return merged;
        }

        public IEnumerator<Setting> GetEnumerator()
        {
            return this.settings.Values.OrderBy(s => s.Name, Comparer<SettingName>.Default).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public virtual SettingsCollection Freeze()
        {
            return new ImmutableSettingsCollection(this);
        }
    }
}