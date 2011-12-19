namespace TecX.Agile.Modules.Gestures.Recognition
{
    using TecX.Common;

    public class AddStoryCardStrategy : GestureRecognitionStrategy
    {
        public override void Process(GestureRecognitionContext context)
        {
            Guard.AssertNotNull(context, "context");
        }
    }
}
