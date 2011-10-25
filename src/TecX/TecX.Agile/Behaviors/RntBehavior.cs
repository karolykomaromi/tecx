using TecX.Agile.ViewModels;

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

        private TranslateOnlyAreaViewModel Toa { get; set; }

        public RntBehavior()
        {
            Toa = new TranslateOnlyAreaViewModel();
            Toa.Visible = false;
        }

        private void AddTranslateOnlyArea(object sender, RoutedEventArgs e)
        {
            Card.Decorators.Add(Toa);

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

            Card.Decorators.Remove(Toa);

            Card = null;
        }

        /// <summary>
        /// Hide translate-only-area
        /// </summary>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Toa != null)
            {
                Toa.Visible = false;
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
                Toa.Visible = true;
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
            Point click = e.GetPosition(Toa.InputArea);

            if (Toa != null && Toa.IsInsideTranslateOnlyArea(click)) 
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
