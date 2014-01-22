namespace Infrastructure
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class DoubleClickTrigger : TriggerBase<FrameworkElement>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                InvokeActions(null);
            }
        }
    }
}
