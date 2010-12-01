using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using TecX.Agile.Infrastructure;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class RntHandler : BehaviorHandler
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly TranslateOnlyArea _toa;

        private bool _isTranslated;
        private Point _previous;

        #endregion Fields

        #region c'tor

        public RntHandler()
        {
            _toa = new TranslateOnlyArea();

            _isTranslated = false;
            _eventAggregator = EventAggregatorAccessor.EventAggregator;
        }

        #endregion c'tor

        #region Overrides of BehaviorHandler

        protected override void DoAttach(FrameworkElement element)
        {
            element.Loaded += AddTranslateOnlyArea;

            Element.MouseEnter += OnMouseEnter;
            Element.MouseLeave += OnMouseLeave;

            Element.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            Element.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            Element.PreviewMouseMove += OnMouseMove;

            ViewModel.StoryCard storyCard = Element.DataContext as ViewModel.StoryCard;

            if(storyCard != null)
            {
                InitializePositionBindings(element, storyCard);
            }
        }

        protected override void DoDetach()
        {
            Element.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            Element.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            Element.PreviewMouseMove -= OnMouseMove;

            Element.MouseEnter -= OnMouseEnter;
            Element.MouseLeave -= OnMouseLeave;

            UserControl ctrl = Element as UserControl;

            if (ctrl != null)
            {
                Panel layoutRoot = ctrl.Content as Panel;

                if (layoutRoot != null)
                {
                    layoutRoot.Children.Remove(_toa);
                }
            }
        }

        #endregion Overrides of BehaviorHandler

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
                !Element.IsPinned())
                _toa.Visibility = Visibility.Visible;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //ignore click if the item is pinned
            if (Element.IsPinned() || 
                !Element.IsMouseCaptured)
                return;

            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                //the actual point is set to the absolute coordinates of the mouse click
                Point actual = e.GetPosition(Tabletop.Surface);

                //ApplyRNT(actual);
                Transition by = GeometryHelper.CalculateRntStep(actual, _previous, 
                    Element.CenterOnSurface(), _isTranslated);

                Element.Move(by.X, by.Y, by.Angle);

                //after the movement set the previous point to the coordinates of the last mouse event
                _previous = actual;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ignore click if the item is pinned
            if (Element.IsPinned())
                return;

            //the item is no longer moved
            _isTranslated = false;

            //release the captured mouse input
            Element.ReleaseMouseCapture();

            //fire the drop-event

            Point dropPoint = e.GetPosition(Tabletop.Surface);
            dropPoint = Tabletop.Surface.PointToScreen(dropPoint);

            _eventAggregator.Publish(new ItemDropped(Element, dropPoint));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if the item is pinned do nothing
            if (Element.IsPinned())
                return;

            //set the previous mouse point to the absolute coordinates of the event
            _previous = e.GetPosition(Tabletop.Surface);

            //if the click occured inside the translate only area the movement is translate-only
            if (_toa != null && _toa.IsInsideTranslateOnlyArea(e.GetPosition(_toa)))
                _isTranslated = true;

            //capture the mouse input

            if (Element.IsEnabled)
            {
                Element.CaptureMouse();
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

        private static void InitializePositionBindings(FrameworkElement element, ViewModel.StoryCard storyCard)
        {
            Binding bindingX = new Binding(Constants.PropertyNames.X)
            {
                Source = storyCard,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            BindingExpressionBase hookForInitialUpdate = BindingOperations.SetBinding(
                element.Translation(),
                TranslateTransform.XProperty,
                bindingX);

            hookForInitialUpdate.UpdateTarget();

            Binding bindingY = new Binding(Constants.PropertyNames.Y)
            {
                Source = storyCard,
                Mode = BindingMode.TwoWay
            };

            hookForInitialUpdate = BindingOperations.SetBinding(
                element.Translation(),
                TranslateTransform.YProperty,
                bindingY);

            hookForInitialUpdate.UpdateTarget();

            Binding bindingAngle = new Binding(Constants.PropertyNames.Angle)
            {
                Source = storyCard,
                Mode = BindingMode.TwoWay
            };

            hookForInitialUpdate = BindingOperations.SetBinding(
                element.Rotation(),
                RotateTransform.AngleProperty,
                bindingAngle);

            hookForInitialUpdate.UpdateTarget();
        }
    }



    internal class ItemDropped
    {
        private readonly FrameworkElement _element;
        private readonly Point _dropPoint;

        public Point DropPoint
        {
            get { return _dropPoint; }
        }

        public FrameworkElement Element
        {
            get { return _element; }
        }

        public ItemDropped(FrameworkElement element, Point dropPoint)
        {
            Guard.AssertNotNull(element, "element");

            _element = element;
            _dropPoint = dropPoint;
        }
    }
}
