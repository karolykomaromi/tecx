namespace Infrastructure.Dynamic
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    public class DynamicView
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(DynamicView),
            new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(FrameworkElement element, bool isEnabled)
        {
            Contract.Requires(element != null);

            element.SetValue(IsEnabledProperty, isEnabled);
        }

        public static bool GetIsEnabled(FrameworkElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;

            IViewRegistry registry = ViewRegistry.Current;
            
            registry.Register(element);
        }
    }
}