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

            for (double angle = 0.0; angle < 360.0; angle += 5.0)
            {
                matrix.Rotate(angle);

                double rotationAngle = GeometryHelper.GetRotationAngleFromMatrix(matrix);
                
                Assert.IsTrue(EpsilonComparer.AreEqual(angle, rotationAngle));

                matrix = Matrix.Identity;
            }

            matrix = Matrix.Identity;
            matrix.Rotate(123.0);
            matrix.Scale(2.3, 4.5);
            matrix.Translate(136, 234);

            double a = GeometryHelper.GetRotationAngleFromMatrix(matrix);

            Assert.IsTrue(EpsilonComparer.AreEqual(123.0, a));


            //Matrix matrix = Matrix.Identity;

            //matrix.Rotate(270);

            //double degrees = GetRotationAngleFromMatrix(matrix);

            //Assert.IsTrue(EpsilonComparer.AreEqual(270, degrees));

            //matrix = Matrix.Identity;

            //matrix.Rotate(90);

            //double degrees2 = GetRotationAngleFromMatrix(matrix);

            //Assert.IsTrue(EpsilonComparer.AreEqual(90, degrees2));

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

            double scaleX = GeometryHelper.GetScaleFactorX(matrix);
            double scaleY = GeometryHelper.GetScaleFactorY(matrix);

            Assert.AreEqual(100, scaleX);
            Assert.AreEqual(150, scaleY);
        }
    }
}
