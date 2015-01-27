namespace Hydra.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class SupportedLanguageElementCollection : ConfigurationElementCollection, IEnumerable<SupportedLanguageElement>
    {
        IEnumerator<SupportedLanguageElement> IEnumerable<SupportedLanguageElement>.GetEnumerator()
        {
            return this.OfType<SupportedLanguageElement>().GetEnumerator();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedLanguageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SupportedLanguageElement)element).Culture.ToString();
        }
    }
}