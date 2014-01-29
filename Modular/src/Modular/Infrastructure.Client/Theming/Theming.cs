namespace Infrastructure.Theming
{
    using System.Windows;

    public class Theming
    {
        public static readonly DependencyProperty Style = DependencyProperty.RegisterAttached(
            "Style",
            typeof(Style),
            typeof(Theming),
            new PropertyMetadata(null, OnStyleChanged));

        public static Style GetStyle(FrameworkElement element)
        {
            return (Style)element.GetValue(Style);
        }

        public static void SetStyle(FrameworkElement element, Style style)
        {
            element.SetValue(Style, style);
        }

        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            Style style = e.NewValue as Style;

            if (element == null || e.NewValue == null)
            {
                return;
            }

            var adapter = new ThemeStyleAdapter(element, style);

            ThemingManager.ThemeChanged += adapter.OnThemeChanged;

            element.Style = style;
        }
    }
}
