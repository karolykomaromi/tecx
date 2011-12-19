namespace TecX.Agile.Modules.Gestures.Recognition
{
    using System.Windows.Ink;

    using TecX.Common;

    public class GestureRecognitionContext
    {
        private readonly ApplicationGesture applicationGesture;

        private readonly StrokeCollection strokes;

        public GestureRecognitionContext(ApplicationGesture applicationGesture, StrokeCollection strokes)
        {
            Guard.AssertNotNull(strokes, "strokes");

            this.applicationGesture = applicationGesture;
            this.strokes = strokes;
        }

        public StrokeCollection Strokes
        {
            get
            {
                return this.strokes;
            }
        }

        public ApplicationGesture ApplicationGesture
        {
            get
            {
                return this.applicationGesture;
            }
        }

        public object Message { get; set; }

        public bool RecognitionCompleted { get; set; }
    }
}
