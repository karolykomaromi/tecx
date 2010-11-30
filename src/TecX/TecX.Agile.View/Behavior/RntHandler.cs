using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TecX.Agile.Infrastructure;
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

        private void AddTranslateOnlyArea(object sender, RoutedEventArgs e)
        {
            UserControl element = sender as UserControl;

            if (element != null)
            {
                //Grid layoutRoot = element.FindName("LayoutRoot") as Grid;
                Panel layoutRoot = element.Content as Panel;

                if (layoutRoot != null)
                {
                    layoutRoot.Children.Add(_toa);
                }

                element.Loaded -= AddTranslateOnlyArea;
            }
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
                !Element.IsPinned())
                _toa.Visibility = Visibility.Visible;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //ignore click if the item is pinned
            if (Element.IsPinned() || !Element.IsMouseCaptured)
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

            //EventAggregator.GetEvent<TabletopItemDropEvent>().Publish(new TabletopItemDropEventArgs(Element, dropPoint));
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
    }

    internal class ItemDropped
    {
        public ItemDropped(FrameworkElement element, Point dropPoint)
        {
            throw new NotImplementedException();
        }
    }

    static class Tabletop
    {
        public static Canvas Surface { get { throw new NotImplementedException(); } }
    }
}
