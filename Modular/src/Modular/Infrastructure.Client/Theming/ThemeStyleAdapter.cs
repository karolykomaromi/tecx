namespace Infrastructure.Theming
{
    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class ThemeStyleAdapter
    {
        private readonly FrameworkElement element;
        private Style style;
        private object styleKey;
        private Action beforeThemeChanged;

        public ThemeStyleAdapter(FrameworkElement element, Style style)
        {
            Contract.Requires(element != null);
            Contract.Requires(style != null);

            this.element = element;
            this.style = style;

            this.element.LayoutUpdated += this.OnLayoutUpdated;
        }

        public void OnThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            Contract.Requires(args != null);
            Contract.Requires(args.ThemeUri != null);

            this.beforeThemeChanged();

            Style newStyle = Application.Current.Resources[this.styleKey] as Style;

            if (newStyle != null)
            {
                this.element.Style = newStyle;
            }
        }

        private static object GetStyleKey(ResourceDictionary dictionary, Style style)
        {
            foreach (DictionaryEntry resource in dictionary)
            {
                if (resource.Value == style)
                {
                    return resource.Key;
                }
            }

            foreach (ResourceDictionary mergedDictionary in dictionary.MergedDictionaries)
            {
                object key = GetStyleKey(mergedDictionary, style);

                if (key != null)
                {
                    return key;
                }
            }

            return null;
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            this.element.LayoutUpdated -= this.OnLayoutUpdated;

            UserControl uc = VisualTree.FindAncestor<UserControl>(this.element);

            if (uc == null)
            {
                return;
            }

            ResourceDictionary dictionary = uc.Resources.MergedDictionaries
                    .FirstOrDefault(dict => dict.Source != null && dict.Source.ToString().EndsWith("Theme.xaml", StringComparison.OrdinalIgnoreCase));

            this.styleKey = GetStyleKey(Application.Current.Resources, this.style);

            if (dictionary == null)
            {
                return;
            }
            
            if (this.styleKey == null)
            {
                this.styleKey = GetStyleKey(dictionary, this.style);
            }

            this.beforeThemeChanged = () =>
                {
                    UserControl uc1 = uc;
                    uc1.Resources.MergedDictionaries.Remove(dictionary);
                    this.beforeThemeChanged = () => { };
                };

            this.style = null;
        }
    }
}