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
            if (this.AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(this.AssociatedObject))
            {
                this.AssociatedObject.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(this.AssociatedObject))
            {
                this.AssociatedObject.MouseLeftButtonDown -= this.OnMouseLeftButtonDown;
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                this.InvokeActions(null);
            }
        }
    }
}
