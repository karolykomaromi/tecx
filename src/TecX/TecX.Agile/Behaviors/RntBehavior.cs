namespace TecX.Agile.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using Microsoft.Practices.ServiceLocation;

    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Utilities;
    using TecX.Agile.ViewModels;
    using TecX.Event;

    public class RntBehavior : Behavior<UserControl>
    {
        private readonly IEventAggregator eventAggregator;

        public RntBehavior()
        {
            this.Toa = new TranslateOnlyAreaViewModel { Visible = false };

            // can't inject to behavior :(
            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        private Point Previous { get; set; }

        private bool IsTranslated { get; set; }

        private StoryCardViewModel Card { get; set; }

        private TranslateOnlyAreaViewModel Toa { get; set; }

        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += this.AddTranslateOnlyArea;

            this.AssociatedObject.MouseEnter += this.OnMouseEnter;
            this.AssociatedObject.MouseLeave += this.OnMouseLeave;

            this.AssociatedObject.PreviewMouseLeftButtonDown += this.OnMouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseLeftButtonUp += this.OnMouseLeftButtonUp;
            this.AssociatedObject.PreviewMouseMove += this.OnMouseMove;

            this.Card = (StoryCardViewModel)this.AssociatedObject.DataContext;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewMouseLeftButtonDown -= this.OnMouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseLeftButtonUp -= this.OnMouseLeftButtonUp;
            this.AssociatedObject.PreviewMouseMove -= this.OnMouseMove;

            this.AssociatedObject.MouseEnter -= this.OnMouseEnter;
            this.AssociatedObject.MouseLeave -= this.OnMouseLeave;

            this.Card.Decorators.Remove(this.Toa);

            this.Card = null;
        }

        private void AddTranslateOnlyArea(object sender, RoutedEventArgs e)
        {
            this.Card.Decorators.Add(this.Toa);

            this.AssociatedObject.Loaded -= this.AddTranslateOnlyArea;
        }

        /// <summary>
        /// Hide translate-only-area
        /// </summary>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (this.Toa != null)
            {
                this.Toa.Visible = false;
            }
        }

        /// <summary>
        /// Show translate only area
        /// </summary>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Toa != null &&
                !this.Card.IsPinned)
            {
                this.Toa.Visible = true;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // ignore click if the item is pinned
            if (this.Card.IsPinned ||
                !AssociatedObject.IsMouseCaptured)
            {
                return;
            }

            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                // the actual point is set to the absolute coordinates of the mouse click
                Point actual = e.GetPosition(SurfaceBehavior.Surface);

                // ApplyRNT(actual);
                Transition by = GeometryHelper.CalculateRntStep(
                    actual, 
                    this.Previous, 
                    this.AssociatedObject.CenterOnSurface(), 
                    this.IsTranslated);

                this.Card.Move(by.X, by.Y, by.Angle);

                // after the movement set the previous point to the coordinates of the last mouse event
                this.Previous = actual;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // ignore click if the item is pinned
            if (this.Card.IsPinned)
            {
                return;
            }

            // the item is no longer moved
            this.IsTranslated = false;

            // release the captured mouse input
            AssociatedObject.ReleaseMouseCapture();

            // fire the drop-event
            Point dropPoint = e.GetPosition(SurfaceBehavior.Surface);
            dropPoint = SurfaceBehavior.Surface.PointToScreen(dropPoint);

            this.eventAggregator.Publish(new DroppedStoryCard(this.Card.Id, dropPoint.X, dropPoint.Y));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // if the item is pinned do nothing
            if (this.Card.IsPinned)
            {
                return;
            }

            // set the previous mouse point to the absolute coordinates of the event
            this.Previous = e.GetPosition(SurfaceBehavior.Surface);

            // if the click occured inside the translate only area the movement is translate-only
            Point click = e.GetPosition(this.Toa.InputArea);

            if (this.Toa != null && this.Toa.IsInsideTranslateOnlyArea(click)) 
            {
                this.IsTranslated = true;
            }

            // capture the mouse input
            if (AssociatedObject.IsEnabled)
            {
                AssociatedObject.CaptureMouse();
            }
        }
    }
}
