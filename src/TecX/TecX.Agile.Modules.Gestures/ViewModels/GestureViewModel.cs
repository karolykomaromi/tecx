using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Common;

namespace TecX.Agile.Modules.Gestures.ViewModels
{
    public class GestureViewModel : Screen
    {
        private readonly IShell _shell;

        private readonly ApplicationGesture[] _addStoryCardGestures = new[]
                                                    {
                                                        ApplicationGesture.ArrowUp,
                                                        ApplicationGesture.ArrowLeft,
                                                        ApplicationGesture.ArrowRight,
                                                        ApplicationGesture.ArrowDown,
                                                        ApplicationGesture.ChevronUp,
                                                        ApplicationGesture.ChevronRight,
                                                        ApplicationGesture.ChevronLeft,
                                                        ApplicationGesture.ChevronDown
                                                    };

        public GestureViewModel(IShell shell)
        {
            Guard.AssertNotNull(shell, "shell");

            _shell = shell;
        }

        public void Gesture(InkCanvasGestureEventArgs e)
        {
            var recognitionResults = e.GetGestureRecognitionResults();

            if (recognitionResults.IsEmpty() ||
                recognitionResults[0].RecognitionConfidence < RecognitionConfidence.Strong)
            {
                return;
            }

            ApplicationGesture gesture = recognitionResults[0].ApplicationGesture;

            Point gestureCenter = GetGestureCenter(e.Strokes.GetBounds());

            switch (gesture)
            {
                case ApplicationGesture.ChevronUp:
                case ApplicationGesture.ArrowUp:
                    AddStoryCardUp(gestureCenter);
                    break;
                default:
                    return;
            }

            //if (IsAddStoryCardGesture(topGesture))
            //{
            //    Point gestureCenter = GetGestureCenter(e.Strokes.GetBounds());

            //    AddStoryCard(gestureCenter, topGesture);
            //    return;
            //}

            //if (topGesture == ApplicationGesture.LeftDown)
            //{
            //    Undo();
            //    return;
            //}

            //if (topGesture == ApplicationGesture.RightDown)
            //{
            //    Redo();
            //    return;
            //}

            //if (topGesture == ApplicationGesture.Square)
            //{
            //    ProcessAddIterationGesture(e, topGesture);
            //    return;
            //}

            //try to interpret the gesture as lasso
            Lasso(e);
        }

        private void AddStoryCardUp(Point gestureCenter)
        {
            _shell.AddStoryCard(gestureCenter.X, gestureCenter.Y, 0.0);
        }

        private void Redo()
        {
        }

        private void Undo()
        {
        }

        private void Lasso(InkCanvasGestureEventArgs e)
        {
            //if (e.Strokes.Count == 1)
            //{
            //    Stroke stroke = e.Strokes[0];

            //    Point[] points = (Point[])stroke.StylusPoints;

            //    List<FrameworkElement> itemsWithCenterInsideGesture = new List<FrameworkElement>();

            //    foreach (var item in Tabletop.Surface.Children)
            //    {
            //        FrameworkElement element = item as FrameworkElement;

            //        if (element != null)
            //        {
            //            if (GeometryHelper.IsPointInsidePolygon(element.CenterOnSurface(), points))
            //            {
            //                itemsWithCenterInsideGesture.Add(element);
            //            }
            //        }
            //    }

            //    //TODO do the grouping thing here

            //    //if (!TypeHelper.IsEmpty(itemsWithCenterInsideGesture))
            //    //{
            //    //    //TODO group and order all items -> order only story-cards?

            //    //    var storycards = from item in itemsWithCenterInsideGesture
            //    //                     where StoryCardBehavior.GetStoryCard(item) != null
            //    //                     select item;

            //    //    //Selects all story-cards
            //    //    //var storycards = from item in itemsWithCenterInsideGesture
            //    //    //            from str in item.Strategies
            //    //    //            where str.GetType() == typeof(StoryCardStrategy)
            //    //    //            select str as StoryCardStrategy;

            //    //    ControlHelper.LayoutTabletopItems(storycards);
            //    //}
            //}
        }

        #region Methods

        private bool IsAddStoryCardGesture(ApplicationGesture topGesture)
        {
            return _addStoryCardGestures.Contains(topGesture);
        }

        private Point GetGestureCenter(Rect gestureBounds)
        {
            //TODO need to decide which corner of the bounds to use dependent on which gesture was used
            //return gestureBounds.TopLeft;

            var topLeft = _shell.PointFromScreen(gestureBounds.TopLeft);
            var bottomRight = _shell.PointFromScreen(gestureBounds.BottomRight);

            var vector = (bottomRight - topLeft) / 2;

            Point center = topLeft + vector;

            return center;
        }

        private static double GetAngleFromChevron(ApplicationGesture topGesture)
        {
            //check which gesture was used to create the card and adjust the rotation
            //angle appropriately
            switch (topGesture)
            {
                case ApplicationGesture.ArrowDown:
                case ApplicationGesture.ChevronDown:
                    return 180.0;
                case ApplicationGesture.ArrowLeft:
                case ApplicationGesture.ChevronLeft:
                    return 270.0;
                case ApplicationGesture.ArrowRight:
                case ApplicationGesture.ChevronRight:
                    return 90.0;
                default:
                    return 0.0;
            }
        }

        #endregion Methods
    }
}
