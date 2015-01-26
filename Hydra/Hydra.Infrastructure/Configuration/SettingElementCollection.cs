namespace Hydra.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class SettingElementCollection : ConfigurationElementCollection, IEnumerable<SettingElement>
    {
        public Setting this[SettingName name]
        {
            get
            {
                Contract.Requires(name != null);
                Contract.Ensures(Contract.Result<Setting>() != null);

                SettingElement element = this.FirstOrDefault(s => s.SettingName.Equals(name));

                Setting setting = element != null ? element.ToSetting() : Setting.Empty;

                return setting;
            }
        }

        public void Add(Setting setting)
        {
            Contract.Requires(setting != null);

            SettingElement element = new SettingElement
            {
                Type = setting.Value.GetType(),
                SettingName = setting.Name,
                Value = setting.Value.ToString()
            };

            this.BaseAdd(element);
        }

        IEnumerator<SettingElement> IEnumerable<SettingElement>.GetEnumerator()
        {
            return this.OfType<SettingElement>().GetEnumerator();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SettingElement)element).SettingName;
        }
    }
}