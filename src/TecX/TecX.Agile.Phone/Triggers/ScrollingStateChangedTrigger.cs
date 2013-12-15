namespace TecX.Agile.Phone.Triggers
{
    using System.Collections;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    public class ScrollingStateChangedTrigger : EventTriggerBase<ListBox>
    {
        private bool alreadyHookedScrollEvents;

        protected override string GetEventName()
        {
            return "ScrollingStateChanged";
        }

        protected override void OnAttached()
        {
            var element = (FrameworkElement)this.AssociatedObject;

            element.Loaded += OnLoaded;
        }

        private static VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(element);

            return groups.OfType<VisualStateGroup>().FirstOrDefault(@group => @group.Name == name);
        }

        private static T FindSimpleVisualChild<T>(DependencyObject element) where T : class
        {
            while (element != null)
            {

                if (element is T)
                {
                    return element as T;
                }

                element = VisualTreeHelper.GetChild(element, 0);
            }

            return null;
        } 

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (alreadyHookedScrollEvents)
                return;

            alreadyHookedScrollEvents = true;
            ScrollViewer viewer = FindSimpleVisualChild<ScrollViewer>(this.AssociatedObject);

            if (viewer != null)
            {
                // Visual States are always on the first child of the control template
                FrameworkElement element = VisualTreeHelper.GetChild(viewer, 0) as FrameworkElement;
                if (element != null)
                {
                    VisualStateGroup group = FindVisualState(element, "ScrollStates");
                    if (group != null)
                    {
                        group.CurrentStateChanging += OnStateChanging;
                    }
                }
            }
        }

        private void OnStateChanging(object s, VisualStateChangedEventArgs args)
        {
            base.OnEvent(args);
        }
    }
}
