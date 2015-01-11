namespace Hydra.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class SupportedLanguagesCollection : ConfigurationElementCollection, IEnumerable<SupportedLanguage>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedLanguage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SupportedLanguage)element).Culture.ToString();
        }

        IEnumerator<SupportedLanguage> IEnumerable<SupportedLanguage>.GetEnumerator()
        {
            return this.OfType<SupportedLanguage>().GetEnumerator();
        }
    }
}