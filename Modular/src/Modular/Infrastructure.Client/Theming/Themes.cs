namespace Infrastructure.Theming
{
    using System.Windows;

    public class Themes
    {
        public static readonly DependencyProperty Enlist = DependencyProperty.RegisterAttached(
            "Enlist",
            typeof(bool),
            typeof(Themes),
            new PropertyMetadata(false, OnFooChanged));

        public static bool GetEnlist(ResourceDictionary dictionary)
        {
            return (bool)dictionary.GetValue(Enlist);
        }

        public static void SetEnlist(ResourceDictionary dictionary, bool value)
        {
            dictionary.SetValue(Enlist, value);
        }

        private static void OnFooChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ResourceDictionary dictionary = d as ResourceDictionary;

            if (d != null)
            {
                var adapter = new ResourceDictionaryAdapter(dictionary);

                ThemingManager.ThemeChanged += adapter.OnThemeChanged;
            }
        }
    }
}
