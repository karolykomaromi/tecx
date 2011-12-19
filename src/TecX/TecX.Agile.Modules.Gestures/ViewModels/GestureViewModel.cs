namespace TecX.Agile.Modules.Gestures.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Ink;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Common;

    public class GestureViewModel : Screen
    {
        private readonly IShell shell;

        private readonly IEventAggregator eventAggregator;

        public GestureViewModel(IShell shell, IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(shell, "shell");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.shell = shell;
            this.eventAggregator = eventAggregator;
        }

        public IShell Shell
        {
            get { return this.shell; }
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

            Point gestureCenter = this.GetGestureCenter(e.Strokes.GetBounds());

            switch (gesture)
            {
                case ApplicationGesture.ChevronUp:
                case ApplicationGesture.ArrowUp:
                    this.Shell.AddStoryCard(Guid.NewGuid(), gestureCenter.X, gestureCenter.Y, 0.0);
                    break;
                default:
                    return;
            }

            ////if (IsAddStoryCardGesture(topGesture))
            ////{
            ////    Point gestureCenter = GetGestureCenter(e.Strokes.GetBounds());

            ////    AddStoryCard(gestureCenter, topGesture);
            ////    return;
            ////}

            ////if (topGesture == ApplicationGesture.LeftDown)
            ////{
            ////    Undo();
            ////    return;
            ////}

            ////if (topGesture == ApplicationGesture.RightDown)
            ////{
            ////    Redo();
            ////    return;
            ////}

            ////if (topGesture == ApplicationGesture.Square)
            ////{
            ////    ProcessAddIterationGesture(e, topGesture);
            ////    return;
            ////}

            ////try to interpret the gesture as lasso

            this.Lasso(e);
        }

        private void Redo()
        {
        }

        private void Undo()
        {
        }

        private void Lasso(InkCanvasGestureEventArgs e)
        {
            ////if (e.Strokes.Count == 1)
            ////{
            ////    Stroke stroke = e.Strokes[0];

            ////    Point[] points = (Point[])stroke.StylusPoints;

            ////    List<FrameworkElement> itemsWithCenterInsideGesture = new List<FrameworkElement>();

            ////    foreach (var item in Tabletop.Surface.Children)
            ////    {
            ////        FrameworkElement element = item as FrameworkElement;

            ////        if (element != null)
            ////        {
            ////            if (GeometryHelper.IsPointInsidePolygon(element.CenterOnSurface(), points))
            ////            {
            ////                itemsWithCenterInsideGesture.Add(element);
            ////            }
            ////        }
            ////    }

            ////    //TODO do the grouping thing here

            ////    //if (!TypeHelper.IsEmpty(itemsWithCenterInsideGesture))
            ////    //{
            ////    //    //TODO group and order all items -> order only story-cards?

            ////    //    var storycards = from item in itemsWithCenterInsideGesture
            ////    //                     where StoryCardBehavior.GetStoryCard(item) != null
            ////    //                     select item;

            ////    //    //Selects all story-cards
            ////    //    //var storycards = from item in itemsWithCenterInsideGesture
            ////    //    //            from str in item.Strategies
            ////    //    //            where str.GetType() == typeof(StoryCardStrategy)
            ////    //    //            select str as StoryCardStrategy;

            ////    //    ControlHelper.LayoutTabletopItems(storycards);
            ////    //}
            ////}
        }

        private Point GetGestureCenter(Rect gestureBounds)
        {
            // TODO need to decide which corner of the bounds to use dependent on which gesture was used
            var topLeft = this.Shell.PointFromScreen(gestureBounds.TopLeft);
            var bottomRight = this.Shell.PointFromScreen(gestureBounds.BottomRight);

            var vector = (bottomRight - topLeft) / 2;

            Point center = topLeft + vector;

            return center;
        }
    }
}
