namespace Infrastructure.Triggers
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class DoubleClickTrigger : TriggerBase<FrameworkElement>
    {
        public bool HandledEventsToo { get; set; }

        protected override void OnAttached()
        {
            if (this.AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(this.AssociatedObject))
            {
                if (this.HandledEventsToo)
                {
                    this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown), true);
                }
                else
                {
                    this.AssociatedObject.MouseLeftButtonDown += this.OnMouseLeftButtonDown;
                }
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(this.AssociatedObject))
            {
                if (this.HandledEventsToo)
                {
                    this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown));
                }
                else
                {
                    this.AssociatedObject.MouseLeftButtonDown -= this.OnMouseLeftButtonDown;
                }
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
