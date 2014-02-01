namespace Infrastructure.Theming
{
    using System.ComponentModel;
    using System.Windows;
    using Infrastructure.Events;

    public class Theming
    {
        public static readonly DependencyProperty StyleProperty = DependencyProperty.RegisterAttached(
            "Style",
            typeof(Style),
            typeof(Theming),
            new PropertyMetadata(null, OnStyleChanged));

        public static Style GetStyle(FrameworkElement element)
        {
            return (Style)element.GetValue(StyleProperty);
        }

        public static void SetStyle(FrameworkElement element, Style style)
        {
            element.SetValue(StyleProperty, style);
        }

        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            FrameworkElement element = d as FrameworkElement;
            Style style = e.NewValue as Style;

            if (element == null || e.NewValue == null)
            {
                return;
            }

            var adapter = new ThemeStyleAdapter(element, style);

            EventAggregator.Current.Subscribe(adapter);

            element.Style = style;
        }
    }
}
