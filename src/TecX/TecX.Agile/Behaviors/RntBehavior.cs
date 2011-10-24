using System;
using System.Windows.Media;

using TecX.Agile.ViewModels;
using TecX.Common;

namespace TecX.Agile.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class RntBehavior : Behavior<UserControl>
    {
        private Point Previous { get; set; }

        private bool IsTranslated { get; set; }

        private StoryCardViewModel Card { get; set; }

        private TranslateOnlyAreaView Toa { get; set; }

        public RntBehavior()
        {
            Toa = new TranslateOnlyAreaView();
            Toa.Visibility = Visibility.Hidden;
        }

        private void AddTranslateOnlyArea(object sender, RoutedEventArgs e)
        {
            var panel = AssociatedObject.Content as Panel;

            if (panel != null)
            {
                panel.Children.Add(Toa);
            }

            AssociatedObject.Loaded -= AddTranslateOnlyArea;
        }


        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AddTranslateOnlyArea;

            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;

            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove += OnMouseMove;

            Card = (StoryCardViewModel)AssociatedObject.DataContext;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove -= OnMouseMove;

            AssociatedObject.MouseEnter -= OnMouseEnter;
            AssociatedObject.MouseLeave -= OnMouseLeave;

            Card = null;

            var panel = AssociatedObject.Content as Panel;

            if (panel != null)
            {
                panel.Children.Remove(Toa);
            }

            //UserControl ctrl = Element as UserControl;

            //if (ctrl != null)
            //{
            //    Panel layoutRoot = ctrl.Content as Panel;

            //    if (layoutRoot != null)
            //    {
            //        layoutRoot.Children.Remove(Toa);
            //    }
            //}

            //AdornerLayer layer = AdornerLayer.GetAdornerLayer(AssociatedObject);

            //layer.Remove(Toa);
        }

        /// <summary>
        /// Hide translate-only-area
        /// </summary>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Toa != null)
            {
                Toa.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Show translate only area
        /// </summary>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Toa != null &&
                !Card.IsPinned)
            {
                Toa.Visibility = Visibility.Visible;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //ignore click if the item is pinned
            if (Card.IsPinned ||
                !AssociatedObject.IsMouseCaptured)
            {
                return;
            }

            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                //the actual point is set to the absolute coordinates of the mouse click
                Point actual = e.GetPosition(SurfaceBehavior.Surface);

                //ApplyRNT(actual);
                Transition by = GeometryHelper.CalculateRntStep(actual, Previous, AssociatedObject.CenterOnSurface(),
                                                                IsTranslated);

                Card.Move(by.X, by.Y, by.Angle);

                //after the movement set the previous point to the coordinates of the last mouse event
                Previous = actual;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ignore click if the item is pinned
            if (Card.IsPinned)
            {
                return;
            }

            //the item is no longer moved
            IsTranslated = false;

            //release the captured mouse input
            AssociatedObject.ReleaseMouseCapture();

            //fire the drop-event

            Point dropPoint = e.GetPosition(SurfaceBehavior.Surface);
            dropPoint = SurfaceBehavior.Surface.PointToScreen(dropPoint);

            //EventAggregator.GetEvent<TabletopItemDropEvent>().Publish(new TabletopItemDropEventArgs(AssociatedObject, dropPoint));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if the item is pinned do nothing
            if (Card.IsPinned)
            {
                return;
            }

            //set the previous mouse point to the absolute coordinates of the event
            Previous = e.GetPosition(SurfaceBehavior.Surface);

            //if the click occured inside the translate only area the movement is translate-only
            if (Toa != null && Toa.IsInsideTranslateOnlyArea(e.GetPosition(Toa)))
            {
                IsTranslated = true;
            }

            //capture the mouse input

            if (AssociatedObject.IsEnabled)
            {
                AssociatedObject.CaptureMouse();
            }
        }
    }

    public static class GeometryHelper
    {
        public static Point CenterOnSurface(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            var surface = SurfaceBehavior.Surface;

            var center = element.PointToScreen(element.Center());

            Point centerOnSurface = surface.PointFromScreen(center);

            return centerOnSurface;
        }

        public static Point Center(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double width = GetWidth(element);

            double height = GetHeight(element);

            Point center = new Point(width / 2, height / 2);

            return center;

        }/// <summary>

        public static double GetWidth(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double width;

            if (!Double.IsNaN(element.ActualWidth) &&
                element.ActualWidth != 0)
            {
                width = element.ActualWidth;
            }
            else if (!Double.IsNaN(element.Width) &&
                     element.Width != 0)
            {
                width = element.Width;
            }
            else
            {
                width = 0;
            }

            return width;
        }

        public static double GetHeight(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double height;

            if (!Double.IsNaN(element.ActualHeight) &&
                element.ActualHeight != 0)
            {
                height = element.ActualHeight;
            }
            else if (!Double.IsNaN(element.Height) &&
                     element.Height != 0)
            {
                height = element.Height;
            }
            else
            {
                height = 0;
            }

            return height;
        }

        public static Transition CalculateRntStep(Point actual, Point previous, Point center, bool isTranslated)
        {
            if (isTranslated)
            {
                //just translate the card without rotation
                return new Transition(actual.X - previous.X, actual.Y - previous.Y, 0);
            }

            //get the vector from the center of the item to the position of the previous point
            Vector vStart = previous - center;

            //get the vector from the center of the item to the position of the actual point
            Vector vEnd = actual - center;

            //calculate the angle between the vectors to know how far the item has to be rotated
            double angle = Vector.AngleBetween(vStart, vEnd);

            //dreht man die Vektoren übereinander kann man den Längenunterschied berechnen und als Bruchteil des
            //Vektors zum zweiten Mauspunkt ausdrücken -> scalar
            double scalar = (vEnd.Length - vStart.Length) / vEnd.Length;

            //calculate the part of the vector that is the actual movement
            Vector displacement = vEnd * scalar;

            //move and rotate the 
            return new Transition(displacement.X, displacement.Y, angle);
        }

        /// <summary>
        /// Gets the translation for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TranslateTransform Translation(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (TranslateTransform)((TransformGroup)element.RenderTransform).Children[2];
        }

        /// <summary>
        /// Gets the rotation for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static RotateTransform Rotation(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (RotateTransform)((TransformGroup)element.RenderTransform).Children[0];
        }

        /// <summary>
        /// Gets the scale for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static ScaleTransform Scale(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (ScaleTransform)((TransformGroup)element.RenderTransform).Children[1];
        }
    }

    public struct Transition
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Angle { get; set; }

        public Transition(double x, double y, double angle)
            : this()
        {
            X = x;
            Y = y;
            Angle = angle % 360.0;
        }
    }
}
