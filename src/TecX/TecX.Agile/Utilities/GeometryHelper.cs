namespace TecX.Agile.Utilities
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using TecX.Agile.Behaviors;
    using TecX.Common;
    using TecX.Common.Comparison;

    public static class GeometryHelper
    {
        public static double GetDistanceBetween(Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;

            double dist = (dx * dx) + (dy * dy);

            dist = Math.Sqrt(dist);

            return dist;
        }

        public static Point CenterOnSurface(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            var surface = SurfaceBehavior.Surface;

            var center = element.PointToScreen(element.Center());

            Point centerOnSurface = surface.PointFromScreen(center);

            return centerOnSurface;
        }

        public static Point Center(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double width = GetWidth(element);

            double height = GetHeight(element);

            Point center = new Point(width / 2, height / 2);

            return center;
        }

        public static double GetWidth(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double width;

            if (!double.IsNaN(element.ActualWidth) && !EpsilonComparer.IsAlmostZero(element.ActualWidth))
            {
                width = element.ActualWidth;
            }
            else if (!double.IsNaN(element.Width) && !EpsilonComparer.IsAlmostZero(element.Width))
            {
                width = element.Width;
            }
            else
            {
                width = 0;
            }

            return width;
        }

        public static double GetHeight(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double height;

            if (!double.IsNaN(element.ActualHeight) && !EpsilonComparer.IsAlmostZero(element.ActualHeight))
            {
                height = element.ActualHeight;
            }
            else if (!double.IsNaN(element.Height) && !EpsilonComparer.IsAlmostZero(element.Height))
            {
                height = element.Height;
            }
            else
            {
                height = 0;
            }

            return height;
        }

        public static Transition CalculateRntStep(Point actual, Point previous, Point center, bool isTranslated)
        {
            if (isTranslated)
            {
                // just translate the card without rotation
                return new Transition(actual.X - previous.X, actual.Y - previous.Y, 0);
            }

            // get the vector from the center of the item to the position of the previous point
            Vector vectorStart = previous - center;

            // get the vector from the center of the item to the position of the actual point
            Vector vectorEnd = actual - center;

            // calculate the angle between the vectors to know how far the item has to be rotated
            double angle = Vector.AngleBetween(vectorStart, vectorEnd);

            // dreht man die Vektoren übereinander kann man den Längenunterschied berechnen und als Bruchteil des
            // Vektors zum zweiten Mauspunkt ausdrücken -> scalar
            double scalar = (vectorEnd.Length - vectorStart.Length) / vectorEnd.Length;

            // calculate the part of the vector that is the actual movement
            Vector displacement = vectorEnd * scalar;

            // move and rotate the 
            return new Transition(displacement.X, displacement.Y, angle);
        }

        /// <summary>
        /// Gets the translation for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TranslateTransform Translation(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (TranslateTransform)((TransformGroup)element.RenderTransform).Children[2];
        }

        /// <summary>
        /// Gets the rotation for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static RotateTransform Rotation(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (RotateTransform)((TransformGroup)element.RenderTransform).Children[0];
        }

        /// <summary>
        /// Gets the scale for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static ScaleTransform Scale(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (ScaleTransform)((TransformGroup)element.RenderTransform).Children[1];
        }
    }
}