namespace Infrastructure.Theming
{
    using System.Windows;

    public class ResourceDictionaryAdapter
    {
        private readonly ResourceDictionary dictionary;

        public ResourceDictionaryAdapter(ResourceDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public void OnThemeChanged(object sender, ThemeChangedEventArgs args)
        {
        }
    }
}