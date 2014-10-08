namespace TecX.Agile.Modules.Gestures.Recognition
{
    using System.Windows.Ink;

    using TecX.Common;

    public class GestureRecognitionContext
    {
        private readonly ApplicationGesture gesture;

        private readonly StrokeCollection strokes;

        public GestureRecognitionContext(ApplicationGesture gesture, StrokeCollection strokes)
        {
            Guard.AssertNotNull(strokes, "strokes");

            this.gesture = gesture;
            this.strokes = strokes;
        }

        public StrokeCollection Strokes
        {
            get
            {
                return this.strokes;
            }
        }

        public ApplicationGesture Gesture
        {
            get
            {
                return this.gesture;
            }
        }

        public object Message { get; set; }

        public bool RecognitionCompleted { get; set; }
    }
}
