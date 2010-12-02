using System;
using System.Windows;
using System.Windows.Media;

using TecX.Agile.View.Behavior;
using TecX.Common;

namespace TecX.Agile.View
{
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Gets the translation for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TranslateTransform Translation(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (element.RenderTransform as TransformGroup).Children[2] as TranslateTransform;
        }

        /// <summary>
        /// Gets the rotation for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static RotateTransform Rotation(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (element.RenderTransform as TransformGroup).Children[0] as RotateTransform;
        }

        /// <summary>
        /// Gets the scale for the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static ScaleTransform Scale(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return (element.RenderTransform as TransformGroup).Children[1] as ScaleTransform;
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

            throw new NotImplementedException();
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
        /// <param name="vector">The vector.</param>
        public static void Move(this FrameworkElement element, Vector vector)
        {
            Guard.AssertNotNull(element, "element");

            element.Move(vector.X, vector.Y);
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

            var translation = element.Translation();

            translation.X += dx;
            translation.Y += dy;

            var rotation = element.Rotation();

            rotation.Angle = (rotation.Angle + angle) % 360;
        }
    }
}
