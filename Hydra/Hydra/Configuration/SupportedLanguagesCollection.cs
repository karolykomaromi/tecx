namespace Hydra.Configuration
{
    using System.Configuration;

    public class SupportedLanguagesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedLanguage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SupportedLanguage)element).Culture.ToString();
        }
    }
}