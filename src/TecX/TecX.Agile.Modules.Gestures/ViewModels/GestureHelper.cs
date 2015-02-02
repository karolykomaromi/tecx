namespace TecX.Agile.Modules.Gestures.ViewModels
{
    using System.Windows;

    using TecX.Agile.Infrastructure;

    public static class GestureHelper
    {
        public static Point GetGestureCenter(Rect gestureBounds)
        {
            // TODO need to decide which corner of the bounds to use dependent on which gesture was used
            var topLeft = Surface.Current.PointFromScreen(gestureBounds.TopLeft);
            var bottomRight = Surface.Current.PointFromScreen(gestureBounds.BottomRight);

            var vector = (bottomRight - topLeft) / 2;

            Point center = topLeft + vector;

            return center;
        }
    }
}