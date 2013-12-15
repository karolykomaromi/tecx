using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace TecX.Agile.View.Behavior
{
    /// <summary>
    /// Sets the focus to an element when the left mouse button is pressed.
    /// Used when a transparent element is on top of the current one which and 
    /// cannot be invisible to hit testing
    /// </summary>
    public class PreviewFocusBehavior : System.Windows.Interactivity.Behavior<TextBox>
    {
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