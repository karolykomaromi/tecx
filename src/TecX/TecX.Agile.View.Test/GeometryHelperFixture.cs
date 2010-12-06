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

            //TODO weberse mist! tan hat eine periode von 180° wie kriege ich da jetzt ein Vorzeichen raus?
            //var atan = Math.Atan(-(matrix.M21 / matrix.M11));
            var atan = Math.Atan(matrix.M12 / matrix.M22);

            var degrees = GeometryHelper.ToDegrees(atan);

            Assert.IsTrue(EpsilonComparer.AreEqual(270 % 180, degrees));

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
