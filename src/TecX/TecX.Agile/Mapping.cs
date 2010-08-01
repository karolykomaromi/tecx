using System;

using TecX.Common;

namespace TecX.Agile
{
    public class Mapping
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
    }
}