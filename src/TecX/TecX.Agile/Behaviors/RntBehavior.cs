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

        public RntBehavior()
        {
        }

        private void AddTranslateOnlyArea(object sender, RoutedEventArgs e)
        {
            //if (AssociatedObject != null)
            //{
            //    AdornerLayer layer = AdornerLayer.GetAdornerLayer(AssociatedObject);

            //    Toa = new TranslateOnlyAdorner(AssociatedObject);

            //    layer.Add(Toa);
            //}
        }


        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AddTranslateOnlyArea;

            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;

            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove += OnMouseMove;
        }

        protected override void  OnDetaching()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove -= OnMouseMove;

            AssociatedObject.MouseEnter -= OnMouseEnter;
            AssociatedObject.MouseLeave -= OnMouseLeave;

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
            //if (Toa != null)
            //    Toa.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Show translate only area
        /// </summary>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            //if (Toa != null &&
            //    !AssociatedObject.IsPinned())
            //    Toa.Visibility = Visibility.Visible;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            ////ignore click if the item is pinned
            //if (AssociatedObject.IsPinned() || !AssociatedObject.IsMouseCaptured)
            //    return;

            //if (e.LeftButton.Equals(MouseButtonState.Pressed))
            //{
            //    //the actual point is set to the absolute coordinates of the mouse click
            //    Point actual = e.GetPosition(Tabletop.Surface);

            //    //ApplyRNT(actual);
            //    Transition by = GeometryHelper.CalculateRntStep(actual, Previous, AssociatedObject.CenterOnSurface(),
            //                                                    IsTranslated);

            //    AssociatedObject.Move(by.X, by.Y, by.Angle);

            //    //after the movement set the previous point to the coordinates of the last mouse event
            //    Previous = actual;
            //}
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ////ignore click if the item is pinned
            //if (AssociatedObject.IsPinned())
            //    return;

            ////the item is no longer moved
            //IsTranslated = false;

            ////release the captured mouse input
            //AssociatedObject.ReleaseMouseCapture();

            ////fire the drop-event

            //Point dropPoint = e.GetPosition(Tabletop.Surface);
            //dropPoint = Tabletop.Surface.PointToScreen(dropPoint);

            //EventAggregator.GetEvent<TabletopItemDropEvent>().Publish(new TabletopItemDropEventArgs(AssociatedObject, dropPoint));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ////if the item is pinned do nothing
            //if (AssociatedObject.IsPinned())
            //    return;

            ////set the previous mouse point to the absolute coordinates of the event
            //Previous = e.GetPosition(Tabletop.Surface);

            ////if the click occured inside the translate only area the movement is translate-only
            //if (Toa != null && Toa.IsInsideTranslateOnlyArea(e.GetPosition(Toa)))
            //    IsTranslated = true;

            ////capture the mouse input

            //if (AssociatedObject.IsEnabled)
            //{
            //    AssociatedObject.CaptureMouse();
            //}
        }

        /// <summary>
        /// Applies the RNT-algorithm to the <see cref="FrameworkElement"/> the strategy is currently hooked up to
        /// </summary>
        /// <param name="actual">The location of the mouse pointer as absolute coordinates</param>
        /// <returns><i>false</i> if no part of the item left the window; <i>true</i> otherwise</returns>
        protected bool ApplyRNT(Point actual)
        {
            //if (IsTranslated)
            //{
            //    //just translate the card without rotation
            //    AssociatedObject.Move(actual.X - Previous.X, actual.Y - Previous.Y);
            //}
            //else
            //{
            //    Point ctr = AssociatedObject.CenterOnSurface();

            //    //get the vector from the center of the item to the position of the previous point
            //    Vector vStart = Previous - ctr;

            //    //get the vector from the center of the item to the position of the actual point
            //    Vector vEnd = actual - ctr;

            //    //calculate the angle between the vectors to know how far the item has to be rotated
            //    double angle = Vector.AngleBetween(vStart, vEnd);

            //    //dreht man die Vektoren übereinander kann man den Längenunterschied berechnen und als Bruchteil des
            //    //Vektors zum zweiten Mauspunkt ausdrücken -> scalar
            //    double scalar = (vEnd.Length - vStart.Length)/vEnd.Length;

            //    //calculate the part of the vector that is the actual movement
            //    Vector displacement = vEnd*scalar;

            //    //move and rotate the item
            //    AssociatedObject.Move(displacement.X, displacement.Y, angle);
            //}

            //if (GeometryHelper.IsRelativePointOutsideCanvas(
            //    AssociatedObject.CenterOnSurface(), Tabletop.Surface))
            //{
            //    Vector displacement = GeometryHelper.GetPointOutsideShapeDisplacement(
            //        AssociatedObject.CenterOnSurface(), Tabletop.Surface);

            //    AssociatedObject.Move(displacement);

            //    return true;
            //}

            return false;
        }
    }

    public static class GeometryHelper
    {
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
