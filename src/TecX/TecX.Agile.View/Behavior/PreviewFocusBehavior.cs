using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

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

            var behaviors = Interaction.GetBehaviors(tb);

            if(e.NewValue is bool == false)
            {
                foreach (var pfb in behaviors.ToArray().OfType<PreviewFocusBehavior>())
                {
                    behaviors.Remove(pfb);
                }
            }
            else
            {
                var behavior = new PreviewFocusBehavior();

                behaviors.Add(behavior);  
            }
        }

        #endregion DependencyProperties


        protected override void OnAttached()
        {
            base.OnAttached();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

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