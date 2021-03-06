﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TecX.Agile.View.Behavior;
using TecX.Common;
using TecX.Common.Comparison;

namespace TecX.Agile.View
{
    public static class FrameworkElementExtensions
    {
        public static readonly DependencyProperty TransformProperty = DependencyProperty.RegisterAttached(
            "Transform",
            typeof(IMatrixTransform),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(null));

        public static IMatrixTransform GetTransform(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            IMatrixTransform transform = element.GetValue(TransformProperty) as IMatrixTransform;

            if (transform == null)
            {
                MatrixTransform mt = element.RenderTransform as MatrixTransform;

                transform = new MatrixTransformWrapper(mt);

                SetTransform(element, transform);
            }

            return transform;
        }

        public static void SetTransform(FrameworkElement element, IMatrixTransform value)
        {
            Guard.AssertNotNull(element, "element");
            Guard.AssertNotNull(value, "value");

            element.SetValue(TransformProperty, value);
        }


        public static Point PointToScreen(this FrameworkElement element, Point point)
        {
            Guard.AssertNotNull(element, "element");

            GeneralTransform transform = element.TransformToVisual(Tabletop.Surface);

            Point pointOnScreen = transform.Transform(point);

            return pointOnScreen;

            ///// <summary>
            ///// Converts between relative and absolute coordinates. Where relative means relative to
            ///// an index card and absolute means coordinates inside the application main window.
            ///// </summary>
            ///// <param name="relPoint">The rel point.</param>
            ///// <param name="center">The center.</param>
            ///// <param name="angle">The angle.</param>
            ///// <param name="transX">The trans X.</param>
            ///// <param name="transY">The trans Y.</param>
            ///// <returns></returns>
            //public static Point RelativeToAbsoluteCoordinates(Point relPoint, Point center, double angle, double transX, double transY)
            //{
            //    double x, y;

            //    x = relPoint.X - center.X;
            //    y = relPoint.Y - center.Y;

            //    double x2 = x * Math.Cos(Util.ToRadians(angle)) - y * Math.Sin(Util.ToRadians(angle));
            //    double y2 = y * Math.Cos(Util.ToRadians(angle)) + x * Math.Sin(Util.ToRadians(angle));

            //    x = x2 + center.X;
            //    y = y2 + center.Y;

            //    return new Point(transX + x, transY + y);
            //}
        }

        public static IMatrixTransform Transform(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            return GetTransform(element);
        }

        public static void Move(this FrameworkElement element, double dx, double dy, double angle)
        {
            //ok, this is a really fragile construct!
            //must change matrix for translation and rotation angle along the lines of 
            //property changes i.e. one after the other or the change tracking will blow up
            //and the changes won't be transmitted correctly to remote clients
            Guard.AssertNotNull(element, "element");

            Matrix matrix = element.Transform().Matrix;

            if (!EpsilonComparer.IsAlmostZero(angle))
            {
                Point center = element.CenterOnSurface();

                //matrix.RotateAt(angle, center.X, center.Y);
                matrix = MatrixHelper.RotateAt(matrix, angle, center.X, center.Y);

                element.Transform().Matrix = matrix;
            }

            //matrix.Translate(dx, 0.0);
            matrix.OffsetX += dx;

            element.Transform().Matrix = matrix;

            //matrix.Translate(0.0, dy);
            matrix.OffsetY += dy;

            element.Transform().Matrix = matrix;
        }

        public static Point CenterOnSurface(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            Point center = GeometryHelper.CenterPoint(element);

            Point centerOnSurface = element.PointToScreen(center);

            return centerOnSurface;
        }

        public static bool IsPinned(this FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            bool isPinned = MovementBehaviorBase.GetIsPinned(element);

            return isPinned;
        }
    }

    public class MatrixTransformWrapper : IMatrixTransform
    {
        private readonly MatrixTransform _matrixTransform;

        public MatrixTransformWrapper(MatrixTransform matrixTransform)
        {
            Guard.AssertNotNull(matrixTransform, "matrixTransform");

            _matrixTransform = matrixTransform;
        }

        public Matrix Matrix
        {
            get { return _matrixTransform.Matrix; }
            set
            {
                if (_matrixTransform.Matrix == value)
                    return;

                _matrixTransform.Matrix = value;
                Changed(this, EventArgs.Empty);
            }
        }

        public MatrixTransform Transform
        {
            get { return _matrixTransform; }
        }

        public event EventHandler Changed = delegate { };
    }

    public interface IMatrixTransform
    {
        Matrix Matrix { get; set; }
        MatrixTransform Transform { get; }

        event EventHandler Changed;
    }
}
