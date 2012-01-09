namespace TecX.CaliburnEx
{
    using System;

    using System.Windows;
    using System.Windows.Interactivity;

    public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
    {
        public RoutedEvent RoutedEvent { get; set; }

        protected override void OnAttached()
        {
            Behavior behavior = this.AssociatedObject as Behavior;
            FrameworkElement associatedElement = this.AssociatedObject as FrameworkElement;

            if (behavior != null)
            {
                associatedElement = ((IAttachedObject)behavior).AssociatedObject as FrameworkElement;
            }

            if (associatedElement == null)
            {
                throw new ArgumentException("Routed Event trigger can only be associated to framework elements");
            }

            if (this.RoutedEvent != null)
            {
                associatedElement.AddHandler(this.RoutedEvent, new RoutedEventHandler(this.OnRoutedEvent));
            }
        }

        protected override string GetEventName()
        {
            return this.RoutedEvent.Name;
        }

        private void OnRoutedEvent(object sender, RoutedEventArgs args)
        {
            base.OnEvent(args);
        }
    }
}
