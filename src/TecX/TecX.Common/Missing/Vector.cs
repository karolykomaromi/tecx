namespace System.Windows
{
    public struct Vector
    {
        #region Fields

        private double _x;
        private double _y;

        #endregion Fields

        #region Properties

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion Properties

        #region c'tor

        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        #endregion c'tor

        /// <summary>
        /// Retrieves the angle, expressed in degrees, between the two specified vectors. 
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The angle, in degrees, between <paramref name="vector1"/> and 
        /// <paramref name="vector2"/></returns>
        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            double y = (vector1._x * vector2._y) - (vector2._x * vector1._y);
            double x = (vector1._x * vector2._x) + (vector1._y * vector2._y);
            return (Math.Atan2(y, x) * 57.295779513082323);
        }

        /// <summary>
        /// Gets the length of this vector.
        /// </summary>
        /// <value>The length.</value>
        public double Length
        {
            get { return Math.Sqrt((_x * _x) + (_y * _y)); }
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector and returns 
        /// the resulting <see cref="Vector"/>. 
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying <paramref name="scalar"/> and 
        /// <paramref name="vector"/>.</returns>
        public static Vector operator *(double scalar, Vector vector)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }

        /// <summary>
        /// Multiplies the specified vector by the specified scalar and returns 
        /// the resulting <see cref="Vector"/>. 
        /// </summary>
        /// <param name="vector">The vector to multiply.</param>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <returns>The result of multiplying <paramref name="vector"/> and 
        /// <paramref name="scalar"/>.</returns>
        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }
    }
}

