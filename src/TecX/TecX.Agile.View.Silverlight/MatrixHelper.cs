using System;
using System.Windows.Media;

namespace TecX.Agile.View
{
    /// <summary>
    /// All of these methods are extracted from the BCL using .NET Reflector.
    /// </summary>
    public static class MatrixHelper
    {
        public static Matrix RotateAt(Matrix matrix, double angle, double centerX, double centerY)
        {
            angle = angle % 360.0;

            matrix = Multiply(matrix, CreateRotationRadians(angle * 0.017453292519943295, centerX, centerY));

            return matrix;
        }

        private static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            Matrix matrix = new Matrix(
                (matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21), 
                (matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22), 
                (matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21), 
                (matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22), 
                ((matrix1.OffsetX * matrix2.M11) + (matrix1.OffsetY * matrix2.M21)) + matrix2.OffsetX, 
                ((matrix1.OffsetX * matrix2.M12) + (matrix1.OffsetY * matrix2.M22)) + matrix2.OffsetY);

            return matrix;
        }

        private static Matrix CreateRotationRadians(double angle, double centerX, double centerY)
        {
            Matrix matrix = new Matrix();
            double num2 = Math.Sin(angle);
            double num = Math.Cos(angle);
            double num4 = (centerX * (1.0 - num)) + (centerY * num2);
            double num3 = (centerY * (1.0 - num)) - (centerX * num2);

            matrix.M11 = num;
            matrix.M12 = num2;
            matrix.M21 = -num2;
            matrix.M22 = num;
            matrix.OffsetX = num4;
            matrix.OffsetY = num3;

            //&matrix.SetMatrix(num, num2, -num2, num, num4, num3, 4);

            return matrix;
        }
    }
}