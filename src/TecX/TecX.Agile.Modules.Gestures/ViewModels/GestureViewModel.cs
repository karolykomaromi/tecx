using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

using Caliburn.Micro;

using TecX.Common;

using IEventAggregator = TecX.Common.Event.IEventAggregator;

namespace TecX.Agile.Modules.Gestures.ViewModels
{
    using TecX.Agile.Infrastructure;
    using TecX.Agile.Infrastructure.Error;
    using TecX.Agile.Infrastructure.Events;

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

            if (TypeHelper.IsEmpty(recognitionResults) ||
                recognitionResults[0].RecognitionConfidence < RecognitionConfidence.Strong)
            {
                return;
            }

            ApplicationGesture gesture = recognitionResults[0].ApplicationGesture;

            switch (gesture)
            {
                case ApplicationGesture.ChevronUp:
                case ApplicationGesture.ArrowUp:
                    AddStoryCardUp();
                    break;
                default:
                    throw new NotImplementedException("Recognition of ApplicationGesture not implemented.");
            }

            //if (IsAddStoryCardGesture(topGesture))
            //{
            //    Point gestureCenter = GetGestureCenterOnSurface(e.Strokes.GetBounds());

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

        private void AddStoryCardUp()
        {
        }

        //private void ProcessAddIterationGesture(InkCanvasGestureEventArgs e, ApplicationGesture topGesture)
        //{
        //    Rect bounds = e.Strokes.GetBounds();

        //    Point center = GetGestureCenterOnSurface(e);

        //    Iteration iteration = new IterationBuilder()
        //        .WithID(Guid.NewGuid())
        //        .WithParent(Planner.CurrentProject.ID)
        //        .WithWidth(bounds.Width)
        //        .WithHeight(bounds.Height)
        //        .WithX(center.X - bounds.Width/2)
        //        .WithY(center.Y - bounds.Height/2);

        //    var args = CommandArgsFactory.GetAddArtefactArgs(CommandSource.Local, iteration);

        //    Commands.AddIterationCommand.Execute(args, Application.Current.MainWindow);
        //}

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

        private void AddStoryCard(Point gestureCenter, ApplicationGesture topGesture)
        {
            //calculate center of the gesture
            //TODO weberse make sure this works in multi-monitor setup
            //TODO weberse check if the center is what you need (top-left fits better?)

            //TODO weberse find all containers at the point of the gesture center
            //used to find out wether to add a story-card to the backlog or an iteration

            //TODO weberse must use ID of container at position of gesture as parent ID

            Guid storyCardId = Guid.NewGuid();
            Guid backlogId = Guid.Empty;
            double x = gestureCenter.X;
            double y = gestureCenter.Y;
            double angle = GetAngleFromChevron(topGesture);

            //Guid parentId = Planner.CurrentProject.Backlog.ID;

            //StoryCard storycard = ViewModelFactory.NewStoryCard()
            //    .ToBuilder()
            //    .WithRotationAngle(angle)
            //    .WithX(gestureCenter.X)
            //    .WithY(gestureCenter.Y)
            //    .WithParent(parentId)
            //    .WithID(Guid.NewGuid());

            //storycard.View.X -= storycard.View.Width/2;
            //storycard.View.Y -= storycard.View.Height/2;

            //var args = CommandArgsFactory.GetAddArtefactArgs(CommandSource.Local, storycard);

            //Commands.AddStoryCard.Execute(args, Application.Current.MainWindow);
        }

        #region Methods

        private bool IsAddStoryCardGesture(ApplicationGesture topGesture)
        {
            return _addStoryCardGestures.Contains(topGesture);
        }

        private static Point GetGestureCenterOnSurface(Rect gestureBounds)
        {
            //TODO need to decide which corner of the bounds to use dependent on which gesture was used
            //return gestureBounds.TopLeft;

            //var topLeft = Tabletop.Surface.PointFromScreen(gestureBounds.TopLeft);
            //var bottomRight = Tabletop.Surface.PointFromScreen(gestureBounds.BottomRight);

            //var vector = (bottomRight - topLeft) / 2;

            //Point center = topLeft + vector;

            //return center;

            return new Point();
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
