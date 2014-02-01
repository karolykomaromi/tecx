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
            bool oldValue = (bool)e.OldValue;
            bool newValue = (bool)e.NewValue;

            if (oldValue == newValue)
            {
                return;
            }

            if (!newValue)
            {
                return;
            }

            var registration = new DelayedRegistration(ViewRegistry.Current, (FrameworkElement)d);
        }
    }
}