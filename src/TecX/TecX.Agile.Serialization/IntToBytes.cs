using System;
using System.Runtime.InteropServices;

namespace TecX.Agile.Serialization
{
    /// <summary>
    /// The "IntToBytes" struct call basically takes care of writing the length of the serialized stream which 
    /// follows into the first four bytes - a very common construct in creating streams of bytes that are 
    /// "self - describing".
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct IntToBytes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntToBytes"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public IntToBytes(Int32 value)
        {
            Byte0 = Byte1 = Byte2 = Byte3 = 0;
            Int32 = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntToBytes"/> struct.
        /// </summary>
        /// <param name="b0">The first byte.</param>
        /// <param name="b1">The second byte.</param>
        /// <param name="b2">The third byte.</param>
        /// <param name="b3">The fourth byte.</param>
        public IntToBytes(byte b0, byte b1, byte b2, byte b3)
        {
            Int32 = 0;
            Byte0 = b0;
            Byte1 = b1;
            Byte2 = b2;
            Byte3 = b3;
        }

        [FieldOffset(0)]
        public Int32 Int32;

        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(2)]
        public byte Byte2;

        [FieldOffset(3)]
        public byte Byte3;
    }
}
