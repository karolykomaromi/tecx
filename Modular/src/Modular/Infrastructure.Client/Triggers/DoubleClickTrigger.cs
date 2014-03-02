namespace Infrastructure.Triggers
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class DoubleClickTrigger : TriggerBase<UIElement>
    {
        private readonly MouseButtonEventHandler handler;

        public DoubleClickTrigger()
        {
            this.handler = this.OnMouseLeftButtonDown;
        }

        public bool HandledEventsToo { get; set; }

        protected override void OnAttached()
        {
            if (this.AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(this.AssociatedObject))
            {
                this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, this.handler, this.HandledEventsToo);
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null && !DesignerProperties.GetIsInDesignMode(this.AssociatedObject))
            {
                this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent, this.handler);
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
