using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.View
{
    /// <summary>
    /// Helper class for geometrical calculations
    /// </summary>
    public static class GeometryHelper
    {
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
        ///Gets the center point of a rectangle
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns></returns>
        public static Point CenterPoint(Rect rectangle)
        {
            double cX = rectangle.Width / 2;
            double cY = rectangle.Height / 2;

            return new Point(cX, cY);
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
        /// Gets the polygon centroid.
        /// </summary>
        /// <remarks>The vertices must be in clockwise order!</remarks>
        /// <param name="vertices">The vertices defining the polygon</param>
        /// <returns>The center-point of the polygon</returns>
        public static Point GetPolygonCentroid(Point[] vertices)
        {
            Guard.AssertNotEmpty(vertices, "vertices");

            if (vertices.Length < 3)
                throw new ArgumentOutOfRangeException("vertices",
                                                      "Polygon must consist of 3 or more vertices")
                    .WithAdditionalInfos(
                    new Dictionary<object, object>
                        {
                            {"vertices", vertices}
                        });

            double cx = 0, cy = 0;

            int n = vertices.Length;

            for (int i = 0; i <= n - 1; i++)
            {
                double a = (vertices[i].X * vertices[(i + 1) % n].Y - vertices[(i + 1) % n].X * vertices[i].Y);
                cx += ((vertices[i].X + vertices[(i + 1) % n].X) * a);
                cy += ((vertices[i].Y + vertices[(i + 1) % n].Y) * a);
            }

            double factor = 1 / (6 * GetPolygonArea(vertices));

            Point centroid = new Point(factor * cx, factor * cy);

            return centroid;
        }

        /// <summary>
        /// Centroids the specified coordinates.
        /// </summary>
        /// <param name="polygon">The points of the polygon.</param>
        /// <returns></returns>
        public static Point PolygonCentroid(Point[] polygon)
        {
            Guard.AssertNotNull(polygon, "polygon");
            Guard.AssertIsInRange(polygon.Length, "polygon.Length", 3, Int32.MaxValue, "Cannot compute " +
                                                                                     "centroid of degenerated polygon (polygon with less than 3 vertices).");

            if (polygon[0] != polygon.Last())
            {
                Point[] closedPolygon = new Point[polygon.Length + 1];
                polygon.CopyTo(closedPolygon, 0);
                closedPolygon[closedPolygon.Length - 1] = polygon[0];
                polygon = closedPolygon;
            }

            int n = polygon.Length;

            // computing area using Green's theorem in a plane and polygon decomposition
            // will allow us to also compute the polygon centroid

            double aSum = 0.0;
            double xSum = 0.0;
            double ySum = 0.0;

            for (int i = 0; i < n - 1; i++)
            {
                int j = i + 1;

                double temp = (polygon[i].X * polygon[j].Y) -
                              (polygon[j].X * polygon[i].Y);
                aSum += temp;

                xSum += (polygon[j].X + polygon[i].X) * temp;
                ySum += (polygon[j].Y + polygon[i].Y) * temp;
            }

            //maybe this might become an OUT parameter?
            //double area = 0.5*aSum;

            Guard.AssertCondition(aSum != 0, aSum, "aSum", "Area of the polygon must not be zero!");

            return new Point(xSum / (aSum * 3.0), ySum / (aSum * 3.0));
        }

        /// <summary>
        /// Gets the area of a polygon
        /// </summary>
        /// <param name="vertices">The vertices of the polygon</param>
        /// <returns>The area of the polygon</returns>
        public static double GetPolygonArea(Point[] vertices)
        {
            Guard.AssertNotEmpty(vertices, "vertices");

            Guard.AssertIsInRange(vertices.Length, "vertices.Length", 3, Int32.MaxValue,
                                  "Polygon must consist of 3 or more vertices");

            double a = 0;
            int n = vertices.Length;

            for (int i = 0; i <= n - 1; i++)
            {
                a += (vertices[i].X * vertices[(i + 1) % n].Y - vertices[(i + 1) % n].X * vertices[i].Y);
            }

            return Math.Abs(a / 2);
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
        /// Converts coordinates between the coordinate systems of two <see cref="FrameworkElement"/>s
        /// </summary>
        /// <param name="point">The coordinates</param>
        /// <param name="origin">The origin of the coordinates</param>
        /// <param name="target">The target of the coordinates</param>
        /// <returns>The converted coordinates</returns>
        public static Point Rel2RelCoords(Point point, FrameworkElement origin, FrameworkElement target)
        {
            Guard.AssertNotNull(origin, "origin");
            Guard.AssertNotNull(target, "target");

            Point absPoint = origin.PointToScreen(point);
            Point relPoint = target.PointFromScreen(absPoint);

            return relPoint;
        }

        /// <summary>
        /// Checks wether a coordinate is outside a <see cref="Canvas"/>
        /// </summary>
        /// <param name="point">The coordinates</param>
        /// <param name="canvas">The canvas</param>
        /// <returns><i>true</i> if the coordinates are outside the canvas; <i>false</i> otherwise</returns>
        public static bool IsRelativePointOutsideCanvas(Point point, Canvas canvas)
        {
            Guard.AssertNotNull(canvas, "canvas");

            double width = GetWidth(canvas);
            double height = GetHeight(canvas);

            if ((point.X < 0) ||
                (point.X > width) ||
                (point.Y < 0) ||
                (point.Y > height))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a point (in absolute coordinates) is inside a given visual element
        /// </summary>
        /// <param name="pointFromScreen">The point from screen.</param>
        /// <param name="element">The element.</param>
        /// <returns>
        /// 	<c>true</c> if [is point from screen outside element] [the specified point from screen]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPointFromScreenInsideElement(Point pointFromScreen, FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            Point relative = element.PointFromScreen(pointFromScreen);

            double width = GetWidth(element);
            double height = GetHeight(element);

            return TypeHelper.IsInRange(relative.X, 0, width) &&
                   TypeHelper.IsInRange(relative.Y, 0, height);
        }

        /// <summary>
        /// Gets how far outside of a canvas a coordinate is
        /// </summary>
        /// <param name="p">The coordinates</param>
        /// <param name="canvas">The canvas</param>
        /// <returns>The vector indicating how far outside the canvas the coordinates are</returns>
        public static Vector GetPointOutsideShapeDisplacement(Point p, Canvas canvas)
        {
            Guard.AssertNotNull(canvas, "canvas");

            double width = GetWidth(canvas);
            double height = GetHeight(canvas);

            Vector displacement = new Vector();

            if (p.X < 0)
                displacement.X = -p.X;
            else if (p.X > width)
                displacement.X = width - p.X;

            if (p.Y < 0)
                displacement.Y = -p.Y;
            else if (p.Y > height)
                displacement.Y = height - p.Y;

            return displacement;
        }


        /// <summary>
        /// Determines whether [is point inside polygon] [the specified p].
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="polygon">The polygon.</param>
        /// <returns>
        /// 	<c>true</c> if [is point inside polygon] [the specified p]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPointInsidePolygon(Point p, Point[] polygon)
        {
            Guard.AssertNotEmpty(polygon, "polygon");

            return WindingNumber(p, polygon) != 0;
        }

        /// <summary>
        /// Winding number test for a point in a polygon
        /// </summary>
        /// <param name="p">A point</param>
        /// <param name="v">Vertex points of a polygon v[n+1] with v[n]=v[0]</param>
        /// <returns>The winding number; <c>0</c> if <c>p</c> is outside <c>v</c></returns>
        private static int WindingNumber(Point p, Point[] v)
        {
            /*
             * Algorithm copied and adapted from
             * 
             * http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm#wn_PinPolygon%28%29
             */

            if (v[0] != v[v.Length - 1])
            {
                Point[] newPolygon = new Point[v.Length + 1];

                Array.Copy(v, 0, newPolygon, 0, v.Length);

                newPolygon[newPolygon.Length - 1] = newPolygon[0];

                v = newPolygon;
            }

            int wn = 0; // the winding number counter
            int n = v.Length - 1;

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {
                // edge from v[i] to v[i+1]
                if (v[i].Y <= p.Y)
                {
                    // start y <= p.y
                    if (v[i + 1].Y > p.Y) // an upward crossing
                        if (IsLeft(v[i], v[i + 1], p) > 0) // p left of edge
                            ++wn; // have a valid up intersect
                }
                else
                {
                    // start y > p.y (no test needed)
                    if (v[i + 1].Y <= p.Y) // a downward crossing
                        if (IsLeft(v[i], v[i + 1], p) < 0) // p right of edge
                            --wn; // have a valid down intersect
                }
            }
            return wn;
        }

        // IsLeft(): tests if a point is Left|On|Right of an infinite line.
        //    Input:  three points P0, P1, and P2
        //    Return: >0 for P2 left of the line through P0 and P1
        //            =0 for P2 on the line
        //            <0 for P2 right of the line
        //    See: the January 2001 Algorithm "Area of 2D and 3D Triangles and Polygons"

        /// <summary>
        /// Determines whether the specified p0 is left.
        /// </summary>
        /// <param name="p0">The p0.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        private static double IsLeft(Point p0, Point p1, Point p2)
        {
            return ((p1.X - p0.X) * (p2.Y - p0.Y)
                    - (p2.X - p0.X) * (p1.Y - p0.Y));
        }

        /// <summary>
        /// Rotates a point about an arbitrary center
        /// </summary>
        /// <param name="p">The point</param>
        /// <param name="centerOfRotation">The center of rotation.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static Point RotateAboutPoint(Point p, Point centerOfRotation, double angle)
        {
            Matrix matrix = Matrix.Identity;

            matrix.RotateAt(angle, centerOfRotation.X, centerOfRotation.Y);

            Point rotatedPoint = matrix.Transform(p);

            return rotatedPoint;
        }

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
            Vector vStart = previous - center;

            //get the vector from the center of the item to the position of the actual point
            Vector vEnd = actual - center;

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

        public static double GetScaleFactorY(Matrix matrix)
        {
            return Math.Sign(matrix.M22) * Math.Sqrt((Math.Pow(matrix.M12, 2.0) + Math.Pow(matrix.M22, 2.0)));
        }

        public static double GetScaleFactorX(Matrix matrix)
        {
            return Math.Sign(matrix.M11) * Math.Sqrt((Math.Pow(matrix.M11, 2.0) + Math.Pow(matrix.M21, 2.0)));
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
