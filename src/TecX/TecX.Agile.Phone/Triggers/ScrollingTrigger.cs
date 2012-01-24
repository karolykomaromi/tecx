namespace TecX.Agile.Phone.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    public class ScrollingTrigger : EventTriggerBase<ListBox>
    {
        /// <summary>
        /// ScrollChanged
        /// </summary>
        private const string ScrollChangedEventName = "ScrollChanged";

        /// <summary>
        /// VerticalOffset
        /// </summary>
        private const string VerticalOffsetPropertyName = "VerticalOffset";

        protected override string GetEventName()
        {
            return ScrollChangedEventName;
        }

        protected override void OnAttached()
        {
            FrameworkElement element = (FrameworkElement)this.AssociatedObject;
            
            element.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= this.OnLoaded;

            ScrollViewer scrollViewer = FindChildOfType<ScrollViewer>(element);

            if (scrollViewer == null)
            {
                throw new InvalidOperationException("ScrollViewer not found.");
            }

            var listener = new DependencyPropertyListener();

            listener.Changed += (sender1, e1) =>
                {
                    var scea = new ScrollChangedEventArgs
                        {
                            ExtentHeight = scrollViewer.ExtentHeight,
                            ExtentWidth = scrollViewer.ExtentWidth,
                            HorizontalOffset = scrollViewer.HorizontalOffset,
                            VerticalOffset = scrollViewer.VerticalOffset,
                            ViewportHeight = scrollViewer.ViewportHeight,
                            ViewportWidth = scrollViewer.ViewportWidth
                        };

                    this.OnEvent(scea);
                };

            Binding binding = new Binding(VerticalOffsetPropertyName) { Source = scrollViewer };
            listener.Attach(scrollViewer, binding);
        }

        private static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }

                    queue.Enqueue(child);
                }
            }

            return null;
        }
    }
}