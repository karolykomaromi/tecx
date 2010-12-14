using System.Windows;
using System.Windows.Media;

using TecX.Agile.View.Behavior;
using TecX.Common;
using TecX.Common.Comparison;

namespace TecX.Agile.View
{
    public static class FrameworkElementExtensions
    {
        public static MatrixTransform Transform(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (MatrixTransform) element.RenderTransform;
        }

        /// <summary>
        /// Gets the local center for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static Point Center(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return GeometryHelper.CenterPoint(element);
        }

        /// <summary>
        /// Gets the center point of the element on the surface.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static Point CenterOnSurface(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            var surface = Tabletop.Surface;

            var center = element.PointToScreen(element.Center());

            Point centerOnSurface = surface.PointFromScreen(center);

            return centerOnSurface;
        }


        /// <summary>
        /// Determines whether the specified element is pinned.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// 	<c>true</c> if the specified element is pinned; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPinned(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            bool isPinned = (bool)element.GetValue(MovementBehaviorBase.IsPinnedProperty);

            return isPinned;
        }

        /// <summary>
        /// Moves the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="dx">The dx.</param>
        /// <param name="dy">The dy.</param>
        public static void Move(this FrameworkElement element, double dx, double dy)
        {
            Guard.AssertNotNull(element, "element");

            element.Move(dx, dy, 0);
        }

        /// <summary>
        /// Moves the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="dx">The dx.</param>
        /// <param name="dy">The dy.</param>
        /// <param name="angle">The angle.</param>
        public static void Move(this FrameworkElement element, double dx, double dy, double angle)
        {
            Guard.AssertNotNull(element, "element");

            Matrix matrix = element.Transform().Matrix;

            if(!EpsilonComparer.IsAlmostZero(angle))
            {
                Point center = element.CenterOnSurface();

                matrix.RotateAt(angle, center.X, center.Y);
            }

            matrix.Translate(dx, dy);

            element.Transform().Matrix = matrix;
        }
    }
}
