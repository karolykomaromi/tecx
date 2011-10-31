using System.Windows.Ink;

namespace TecX.Agile.Modules.Gestures.ViewModels
{
    public abstract class GestureRecognitionStrategy
    {
        public abstract bool Process(ApplicationGesture gesture, StrokeCollection strokes);
    }
}