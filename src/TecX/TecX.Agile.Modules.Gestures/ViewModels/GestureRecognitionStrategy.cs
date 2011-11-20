namespace TecX.Agile.Modules.Gestures.ViewModels
{
    using System.Windows.Ink;

    public abstract class GestureRecognitionStrategy
    {
        public abstract bool Process(ApplicationGesture gesture, StrokeCollection strokes);
    }
}