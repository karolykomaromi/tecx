using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using TecX.Agile.Infrastructure;
using TecX.Common.Comparison;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class RntBehavior : MovementBehaviorBase
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly TranslateOnlyArea _toa;

        private bool _isTranslated;
        private Point _previous;
        private IDisposable _storyCardChangeSubscription;
        private IDisposable _transformChangeSubscription;

        #endregion Fields

        #region c'tor

        public RntBehavior()
        {
            _toa = new TranslateOnlyArea();

            _isTranslated = false;
            _eventAggregator = EventAggregatorAccessor.EventAggregator;
        }

        #endregion c'tor

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AddTranslateOnlyArea;

            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;

            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove += OnMouseMove;

            ViewModel.StoryCard storyCard = AssociatedObject.DataContext as ViewModel.StoryCard;

            if (storyCard != null)
            {
                InitializePositionBindings(AssociatedObject, storyCard);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseMove -= OnMouseMove;

            AssociatedObject.MouseEnter -= OnMouseEnter;
            AssociatedObject.MouseLeave -= OnMouseLeave;

            if (AssociatedObject != null)
            {
                Panel layoutRoot = AssociatedObject.Content as Panel;

                if (layoutRoot != null)
                {
                    layoutRoot.Children.Remove(_toa);
                }
            }

            _storyCardChangeSubscription.Dispose();
            _storyCardChangeSubscription = null;

            _transformChangeSubscription.Dispose();
            _transformChangeSubscription = null;
        }

        #region EventHandling

        /// <summary>
        /// Hide translate-only-area
        /// </summary>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_toa != null)
                _toa.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Show translate only area
        /// </summary>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (_toa != null &&
                !AssociatedObject.IsPinned())
                _toa.Visibility = Visibility.Visible;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //ignore click if the item is pinned
            if (AssociatedObject.IsPinned() ||
                !AssociatedObject.IsMouseCaptured)
                return;

            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                //the actual point is set to the absolute coordinates of the mouse click
                Point actual = e.GetPosition(Tabletop.Surface);

                //ApplyRNT(actual);
                Transition by = GeometryHelper.CalculateRntStep(actual, _previous,
                    AssociatedObject.CenterOnSurface(), _isTranslated);

                AssociatedObject.Move(by.X, by.Y, by.Angle);

                //after the movement set the previous point to the coordinates of the last mouse event
                _previous = actual;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ignore click if the item is pinned
            if (AssociatedObject.IsPinned())
                return;

            //the item is no longer moved
            _isTranslated = false;

            //release the captured mouse input
            AssociatedObject.ReleaseMouseCapture();

            //fire the drop-event

            Point dropPoint = e.GetPosition(Tabletop.Surface);
            dropPoint = Tabletop.Surface.PointToScreen(dropPoint);

            _eventAggregator.Publish(new ItemDropped(AssociatedObject, dropPoint));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if the item is pinned do nothing
            if (AssociatedObject.IsPinned())
                return;

            //set the previous mouse point to the absolute coordinates of the event
            _previous = e.GetPosition(Tabletop.Surface);

            //if the click occured inside the translate only area the movement is translate-only
            if (_toa != null && _toa.IsInsideTranslateOnlyArea(e.GetPosition(_toa)))
                _isTranslated = true;

            //capture the mouse input

            if (AssociatedObject.IsEnabled)
            {
                AssociatedObject.CaptureMouse();
            }
        }

        #endregion EventHandling

        private void AddTranslateOnlyArea(object sender, RoutedEventArgs e)
        {
            UserControl element = sender as UserControl;

            if (element != null)
            {
                Grid overlay = element.FindName("Overlay") as Grid;
                //Panel layoutRoot = element.Content as Panel;

                if (overlay != null)
                {
                    overlay.Children.Add(_toa);
                }

                element.Loaded -= AddTranslateOnlyArea;
            }
        }

        private void InitializePositionBindings(FrameworkElement element, ViewModel.StoryCard storyCard)
        {
            Matrix initialMatrix = element.Transform().Matrix;

            if (storyCard.X != default(double))
            {
                initialMatrix.OffsetX = storyCard.X;
            }
            else
            {
                storyCard.X = initialMatrix.OffsetX;
            }

            if (storyCard.Y != default(double))
            {
                initialMatrix.OffsetY = storyCard.Y;
            }
            else
            {
                storyCard.Y = initialMatrix.OffsetY;
            }

            if (storyCard.Angle != default(double))
            {
                Point center = element.CenterOnSurface();

                initialMatrix.RotateAt(storyCard.Angle, center.X, center.Y);
            }
            else
            {
                storyCard.Angle = GeometryHelper.GetRotationAngleFromMatrix(initialMatrix);
            }

            var transform = from evt in Observable.FromEvent<EventArgs>(element.Transform(), "Changed")
                            let matrix = ((MatrixTransform)evt.Sender).Matrix
                            let angle = GeometryHelper.GetRotationAngleFromMatrix(matrix)
                            where !EpsilonComparer.AreEqual(storyCard.X, matrix.OffsetX) ||
                                  !EpsilonComparer.AreEqual(storyCard.Y, matrix.OffsetY) ||
                                  !EpsilonComparer.AreEqual(storyCard.Angle, angle)
                            select
                                new
                                {
                                    X = matrix.OffsetX,
                                    Y = matrix.OffsetY,
                                    Angle = angle
                                };

            _transformChangeSubscription = transform.Subscribe(fromMatrix =>
            {
                storyCard.X = fromMatrix.X;
                storyCard.Y = fromMatrix.Y;
                storyCard.Angle = fromMatrix.Angle;
            });

            var position = from evt in Observable.FromEvent<PropertyChangedEventArgs>(storyCard, "PropertyChanged")
                           let name = evt.EventArgs.PropertyName
                           let matrix = element.Transform().Matrix
                           let angle = GeometryHelper.GetRotationAngleFromMatrix(matrix)
                           where (name == Constants.PropertyNames.X && !EpsilonComparer.AreEqual(matrix.OffsetX, storyCard.X)) ||
                                 (name == Constants.PropertyNames.Y && !EpsilonComparer.AreEqual(angle, storyCard.Angle)) ||
                                 (name == Constants.PropertyNames.Angle && !EpsilonComparer.AreEqual(matrix.OffsetY, storyCard.Y))
                           select new
                           {
                               X = storyCard.X - matrix.OffsetX,
                               Y = storyCard.Y - matrix.OffsetY,
                               Angle = storyCard.Angle - angle
                           };

            _storyCardChangeSubscription = position.Subscribe(delta => element.Move(delta.X, delta.Y, delta.Angle));
        }
    }
}
