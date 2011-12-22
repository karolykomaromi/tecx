namespace TecX.Agile.Modules.Gestures.ViewModels
{
    using System.Windows.Controls;
    using System.Windows.Ink;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Modules.Gestures.Recognition;
    using TecX.Common;
    using TecX.Event;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public class GestureViewModel : Screen
    {
        private readonly IShell shell;

        private readonly IEventAggregator eventAggregator;

        private readonly GestureStrategyChain gestureStrategyChain;

        public GestureViewModel(IShell shell, IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(shell, "shell");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.shell = shell;
            this.eventAggregator = eventAggregator;
            this.gestureStrategyChain = new GestureStrategyChain().Initialize();
        }

        public IShell Shell
        {
            get { return this.shell; }
        }

        public void Gesture(InkCanvasGestureEventArgs eventArgs)
        {
            Guard.AssertNotNull(eventArgs, "eventArgs");

            var recognitionResults = eventArgs.GetGestureRecognitionResults();

            if (recognitionResults.IsEmpty() ||
                recognitionResults[0].RecognitionConfidence < RecognitionConfidence.Strong)
            {
                return;
            }

            ApplicationGesture gesture = recognitionResults[0].ApplicationGesture;

            var context = new GestureRecognitionContext(gesture, eventArgs.Strokes);

            this.gestureStrategyChain.Process(context);

            if (context.Message != null)
            {
                var publisher = new MessagePublisher(this.eventAggregator);

                publisher.Publish(context.Message);
            }

            ////Point gestureCenter = GestureHelper.GetGestureCenter(eventArgs.Strokes.GetBounds());

            ////switch (gesture)
            ////{
            ////    case ApplicationGesture.ChevronUp:
            ////    case ApplicationGesture.ArrowUp:
            ////        this.Shell.AddStoryCard(Guid.NewGuid(), gestureCenter.X, gestureCenter.Y, 0.0);
            ////        break;
            ////    default:
            ////        return;
            ////}

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

            this.Lasso(eventArgs);
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
    }
}
