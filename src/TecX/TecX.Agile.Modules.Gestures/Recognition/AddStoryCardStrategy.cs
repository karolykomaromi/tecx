namespace TecX.Agile.Modules.Gestures.Recognition
{
    using System;
    using System.Windows;
    using System.Windows.Ink;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Modules.Gestures.ViewModels;
    using TecX.Common;

    public class AddStoryCardStrategy : GestureRecognitionStrategy
    {
        public override void Process(GestureRecognitionContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.Strokes, "context.Strokes");

            Point gestureCenter = GestureHelper.GetGestureCenter(context.Strokes.GetBounds());

            switch (context.Gesture)
            {
                case ApplicationGesture.ChevronUp:
                case ApplicationGesture.ArrowUp:
                    context.Message = new AddStoryCard(
                        Guid.NewGuid(),
                        gestureCenter.X - (Defaults.StoryCard.Width / 2),
                        gestureCenter.Y - (Defaults.StoryCard.Height / 2),
                        0.0);
                    break;
                default:
                    return;
            }
        }
    }
}
