using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Comparison;

namespace TecX.Agile.View.Test
{
    [TestClass]
    public class GeometryHelperFixture
    {
        [TestMethod]
        public void GivenMatrix_WhenRotating_CanCalculateAngle()
        {
            Matrix matrix = Matrix.Identity;

            matrix.Rotate(270);

            Point point = new Point(0, 100);

            Point p2 = matrix.Transform(point);

            Assert.AreEqual(100, p2.X);

            var atan = Math.Atan(matrix.M12 / matrix.M22);

            var degrees = -GeometryHelper.ToDegrees(atan);

            matrix = Matrix.Identity;

            matrix.Rotate(90);

            atan = Math.Atan(matrix.M12 / matrix.M22);

            double degrees2 = -GeometryHelper.ToDegrees(atan);

            Assert.IsFalse(EpsilonComparer.AreEqual(degrees, degrees2));

        }

        [TestMethod]
        public void GivenMatrix_WhenTranslating_CanCalculateOffsets()
        {
            Matrix matrix = Matrix.Identity;

            matrix.Translate(50, 100);

            Assert.AreEqual(50, matrix.OffsetX);
            Assert.AreEqual(100, matrix.OffsetY);

            Point point = new Point(0, 0);

            Point p2 = matrix.Transform(point);

            Assert.AreEqual(50, p2.X);
            Assert.AreEqual(100, p2.Y);
        }

        [TestMethod]
        public void GivenMatrix_WhenScaling_CanCalculateScaleFactors()
        {
            Matrix matrix = Matrix.Identity;

            matrix.Scale(100, 150);

            Point point = new Point(10, 10);

            Point p2 = matrix.Transform(point);

            Assert.AreEqual(1000, p2.X);
            Assert.AreEqual(1500, p2.Y);

            double scaleX = Math.Sign(matrix.M11) * Math.Sqrt((Math.Pow(matrix.M11, 2.0) + Math.Pow(matrix.M21, 2.0)));
            double scaleY = Math.Sign(matrix.M22) * Math.Sqrt((Math.Pow(matrix.M12, 2.0) + Math.Pow(matrix.M22, 2.0)));

            Assert.AreEqual(100, scaleX);
            Assert.AreEqual(150, scaleY);
        }
    }
}
