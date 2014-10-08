using System;
using System.Windows;
using System.Windows.Media;

using TecX.Common;

namespace TecX.Agile.View
{
    public static class GeometryHelper
    {
        /// <summary>
        /// Calculates a single step of the RNT algorithm
        /// </summary>
        /// <param name="actual">The actual mouse coordinate</param>
        /// <param name="previous">The previous mouse coordinate</param>
        /// <param name="center">The center of the <c>TableTopItem</c></param>
        /// <param name="isTranslated">if set to <c>true</c> the item is only translated not rotated.</param>
        /// <returns>The set of values by which to move and turn the <c>TableTopItem</c></returns>
        public static Transition CalculateRntStep(Point actual, Point previous, Point center, bool isTranslated)
        {
            if (isTranslated)
            {
                //just translate the card without rotation
                return new Transition(actual.X - previous.X, actual.Y - previous.Y, 0);
            }

            //get the vector from the center of the item to the position of the previous point
            Vector vStart = new Vector(previous.X - center.X, previous.Y - center.Y);

            //get the vector from the center of the item to the position of the actual point
            Vector vEnd = new Vector(actual.X - center.X, actual.Y - center.Y);

            //calculate the angle between the vectors to know how far the item has to be rotated
            double angle = Vector.AngleBetween(vStart, vEnd);

            //dreht man die Vektoren übereinander kann man den Längenunterschied berechnen und als Bruchteil des
            //Vektors zum zweiten Mauspunkt ausdrücken -> scalar
            double scalar = (vEnd.Length - vStart.Length) / vEnd.Length;

            //calculate the part of the vector that is the actual movement
            Vector displacement = vEnd * scalar;

            //move and rotate the 
            return new Transition(displacement.X, displacement.Y, angle);
        }


        public static double GetRotationAngleFromMatrix(Matrix matrix)
        {
            double s11 = Math.Sign(matrix.M11);
            double s22 = Math.Sign(matrix.M22);

            double quadrantCorrectionFactor = s11 < 0 && s22 < 0 ? -180.0 : 0;

            double atan = Math.Atan(matrix.M12 / matrix.M22);

            double degrees = ToDegrees(atan);

            degrees = degrees + quadrantCorrectionFactor;

            degrees = degrees < 0 ? degrees + 360.0 : degrees;

            return degrees;
        }

        /// <summary>
        /// Converts an angle expressed in degrees to an angle expressed in radians
        /// </summary>
        /// <param name="degrees">The angle in degrees</param>
        /// <returns>The angle expressed in radians</returns>
        public static double ToRadians(double degrees)
        {
            double angle = (degrees * Math.PI / 180) % 360;

            angle = angle < 0 ? angle + 360.0 : angle;

            return angle;
        }

        /// <summary>
        /// Converts an angle expressed in radians to an angle expressed in degrees
        /// </summary>
        /// <param name="radians">The angle in radians</param>
        /// <returns>The angle expressed in degrees</returns>
        public static double ToDegrees(double radians)
        {
            double angle = (radians * 180 / Math.PI) % 360;

            angle = angle < 0 ? angle + 360.0 : angle;

            return angle;
        }

        /// <summary>
        /// Gets the center point of a framework element
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns></returns>
        public static Point CenterPoint(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double width = GetWidth(element);

            double height = GetHeight(element);

            Point center = new Point(width / 2, height / 2);

            return center;
        }

        /// <summary>
        /// Gets the width of a <see cref="FrameworkElement"/>
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>The width or <i>0.0</i> in case of an error</returns>
        public static double GetWidth(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double width;

            if (!Double.IsNaN(element.ActualWidth) &&
                element.ActualWidth != 0)
            {
                width = element.ActualWidth;
            }
            else if (!Double.IsNaN(element.Width) &&
                     element.Width != 0)
            {
                width = element.Width;
            }
            else
            {
                width = 0;
            }

            return width;
        }

        /// <summary>
        /// Gets the height of a <see cref="FrameworkElement"/>
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>The height or <i>0.0</i> in case of an error</returns>
        public static double GetHeight(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            double height;

            if (!Double.IsNaN(element.ActualHeight) &&
                element.ActualHeight != 0)
            {
                height = element.ActualHeight;
            }
            else if (!Double.IsNaN(element.Height) &&
                     element.Height != 0)
            {
                height = element.Height;
            }
            else
            {
                height = 0;
            }

            return height;
        }

        /// <summary>
        /// Calculates the euclidean distance between two 2-dimensional points
        /// </summary>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <returns>The distance</returns>
        public static double GetDistanceBetween(Point p1, Point p2)
        {
            double diffX = p1.X - p2.X;
            double diffY = p1.Y - p2.Y;

            double dist = diffX * diffX + diffY * diffY;

            dist = Math.Sqrt(dist);

            return dist;
        }
    }

    /// <summary>
    /// Bundles movement coordinates and rotation angle
    /// </summary>
    public struct Transition
    {
        #region Properties

        /// <summary>
        /// Gets or sets the distance by which to move
        /// the item along the X-axis
        /// </summary>
        /// <value>The X.</value>
        public double X { get; private set; }

        /// <summary>
        /// Gets or sets the distance by which to move
        /// the item along the Y-axis
        /// </summary>
        /// <value>The Y.</value>
        public double Y { get; private set; }

        /// <summary>
        /// Gets or sets the angle by which to rotate
        /// the item
        /// </summary>
        /// <value>The angle.</value>
        public double Angle { get; private set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="angle">The angle.</param>
        public Transition(double x, double y, double angle)
            : this()
        {
            X = x;
            Y = y;
            Angle = angle % 360.0;
        }

        #endregion c'tor
    }
}
