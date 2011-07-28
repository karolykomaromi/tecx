using System;
using System.Linq;

using TecX.Common;
using TecX.Common.Comparison;

namespace TecX.Agile
{
    public class Mapping : IEquatable<Mapping>
    {
        public string Name { get; private set; }
        public byte[] Color { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapping"/> class
        /// </summary>
        public Mapping(string name, byte[] color)
        {
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(color, "color");

            if (color.Length != 4)
            {
                throw new ArgumentOutOfRangeException("color", "A color must be an array of 4 bytes (ARGB)");
            }

            Name = name;
            Color = color;
        }

        #region Overrides of Object

        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            Mapping other = obj as Mapping;

            if (other != null)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion Overrides of Object

        #region Implementation of IEquatable<Mapping>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Mapping other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = Compare.AreEqual(Name, other.Name);
            equal &= Color.SequenceEqual(other.Color);

            return equal;
        }

        #endregion Implementation of IEquatable<Mapping>
    }
}