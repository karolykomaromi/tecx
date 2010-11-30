using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TecX.Agile.View.Behavior;

namespace TecX.Agile.View
{
    public static class GeometryHelper
    {
        public static Point CenterPoint(FrameworkElement element)
        {
            throw new NotImplementedException();
        }

        public static double GetDistanceBetween(Point point, Point center)
        {
            throw new NotImplementedException();
        }

        public static double GetWidth(FrameworkElement element)
        {
            throw new NotImplementedException();
        }

        public static bool IsRelativePointOutsideCanvas(object centerOnSurface, object surface)
        {
            throw new NotImplementedException();
        }

        public static Vector GetPointOutsideShapeDisplacement(Point centerOnSurface, object surface)
        {
            throw new NotImplementedException();
        }

        public static Transition CalculateRntStep(Point actual, Point previous, Point centerOnSurface, bool isTranslated)
        {
            throw new NotImplementedException();
        }
    }

    public class Transition
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Angle { get; set; }
    }
}
