using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    /// <summary>
    /// Sets the focus to an element when the left mouse button is pressed.
    /// Used when a transparent element is on top of the current one which and 
    /// cannot be invisible to hit testing
    /// </summary>
    public class PreviewFocusBehavior : System.Windows.Interactivity.Behavior<TextBox>
    {
        #region DependencyProperties

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(PreviewFocusBehavior),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnEnabledChanged));

        public static bool GetIsEnabled(TextBox tb)
        {
            Guard.AssertNotNull(tb, "tb");

            return (bool)tb.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(TextBox tb, bool isEnabled)
        {
            Guard.AssertNotNull(tb, "tb");

            tb.SetValue(IsEnabledProperty, isEnabled);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            TextBox tb = d as TextBox;

            if (tb == null)
                return;

            var behavior = new PreviewFocusBehavior();

            behavior.Attach(tb);
        }

        #endregion DependencyProperties


        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.Focus();
        }
    }
}