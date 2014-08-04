namespace TecX.Agile.Modules.Gestures.Recognition
{
    public abstract class GestureRecognitionStrategy
    {
        public abstract void Process(GestureRecognitionContext context);
    }
}