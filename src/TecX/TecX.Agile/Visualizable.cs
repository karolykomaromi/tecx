using System;
using System.Linq;

using TecX.Common;

namespace TecX.Agile
{
    /// <summary>
    /// Visual attributes like position and orientation
    /// </summary>
    public class Visualizable : Features<Visualizable>
    {
        #region Constants

        public static class Constants
        {
            public static class PropertyNames
            {
                /// <summary>X</summary>
                public const string XPropertyName = "X";

                /// <summary>Y</summary>
                public const string YPropertyName = "Y";

                /// <summary>RotationAngle</summary>
                public const string RotationAnglePropertyName = "RotationAngle";

                /// <summary>Width</summary>
                public const string WidthPropertyName = "Width";

                /// <summary>Height</summary>
                public const string HeightPropertyName = "Height";

                /// <summary>Color</summary>
                public const string ColorPropertyName = "Color";
            }
        }

        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region Fields

        /// <summary>
        /// Backing field for <see cref="Color"/>
        /// </summary>
        private byte[] _color;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        /// <summary>
        /// Gets and sets the x-coordinate of the index-card
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets and sets the y-coordinate of the index-card
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets and sets the rotation angle of the index-card
        /// </summary>
        public double RotationAngle { get; set; }

        /// <summary>
        /// Gets and sets the width of the index-card
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets and sets the height of the index-card
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets and sets the color of the index-card
        /// </summary>
        public byte[] Color
        {
            get { return _color; }
            set
            {
                if (value == null)
                    value = new byte[0];

                _color = value;
            }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Visualizable"/> class.
        /// </summary>
        public Visualizable()
        {
            Color = new byte[0];
            Height = 0;
            RotationAngle = 0;
            Width = 0;
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Visualizable"/> class.
        /// </summary>
        /// <param name="original">The original.</param>
        private Visualizable(Visualizable original)
        {
            Guard.AssertNotNull(original, "original");

            Color = (byte[])original.Color.Clone();
            Height = original.Height;
            RotationAngle = original.RotationAngle;
            Width = original.Width;
            X = original.X;
            Y = original.Y;
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Methods

        /// <summary>
        /// Copies the values of all properties from another visualizable.
        /// </summary>
        /// <param name="other">This visualizable with the new values</param>
        internal void CopyValuesFrom(Visualizable other)
        {
            Guard.AssertNotNull(other, "other");

            Color = (byte[])other.Color.Clone();
            Height = other.Height;
            RotationAngle = other.RotationAngle;
            Width = other.Width;
            X = other.X;
            Y = other.Y;
        }

        #endregion Methods

        ////////////////////////////////////////////////////////////

        #region Overrides of Features

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = new Visualizable(this);

            return clone;
        }

        public override bool Equals(Visualizable other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = Color.SequenceEqual(other.Color);
            equal &= Height == other.Height;
            equal &= RotationAngle == other.RotationAngle;
            equal &= Width == other.Width;
            equal &= X == other.X;
            equal &= Y == other.Y;

            return equal;
        }

        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            Visualizable other = obj as Visualizable;

            if (other != null)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Overrides of Features
    }
}