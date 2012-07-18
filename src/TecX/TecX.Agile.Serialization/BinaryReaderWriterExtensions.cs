using System;
using System.IO;
using System.Windows.Media;

using TecX.Common;

namespace TecX.Agile.Serialization
{
    public static class BinaryReaderWriterExtensions
    {
        public static void Write(this BinaryWriter writer, Guid guid)
        {
            Guard.AssertNotNull(writer, "writer");

            writer.Write(guid.ToByteArray());
        }

        public static void Write(this BinaryWriter writer, decimal value)
        {
            Guard.AssertNotNull(writer, "writer");

            int[] bits = decimal.GetBits(value);

            writer.Write(bits[0]);
            writer.Write(bits[1]);
            writer.Write(bits[2]);
            writer.Write(bits[3]);
        }

        public static void WriteUnkown(this BinaryWriter writer, object value)
        {
            Guard.AssertNotNull(writer, "writer");

            if (value == null)
            {
                writer.Write(Constants.DataTypes.Null);
                return;
            }

            Type type = value.GetType();

            if (type == typeof(decimal))
            {
                writer.Write(Constants.DataTypes.Decimal);
                writer.Write((decimal)value);
                return;
            }

            if (type == typeof(int))
            {
                writer.Write(Constants.DataTypes.Int32);
                writer.Write((int)value);
                return;
            }

            if (type == typeof(string))
            {
                writer.Write(Constants.DataTypes.String);
                writer.Write((string)value);
                return;
            }

            if (type == typeof(Guid))
            {
                writer.Write(Constants.DataTypes.Guid);
                writer.Write((Guid)value);
                return;
            }

            if (type == typeof(double))
            {
                writer.Write(Constants.DataTypes.Double);
                writer.Write((double)value);
                return;
            }

            if (type == typeof(Color))
            {
                writer.Write(Constants.DataTypes.Color);
                writer.Write((Color)value);
                return;
            }

            throw new ArgumentException();
        }

        public static void Write(this BinaryWriter writer, Color value)
        {
            Guard.AssertNotNull(writer, "writer");

            writer.Write(value.A);
            writer.Write(value.R);
            writer.Write(value.G);
            writer.Write(value.B);
        }

        public static object ReadUnknown(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            byte dataType = reader.ReadByte();

            if (dataType == Constants.DataTypes.Null)
                return null;

            if (dataType == Constants.DataTypes.Decimal)
                return reader.ReadDecimal();

            if (dataType == Constants.DataTypes.Int32)
                return reader.ReadInt32();

            if (dataType == Constants.DataTypes.String)
                return reader.ReadString();

            if (dataType == Constants.DataTypes.Guid)
                return reader.ReadGuid();

            if (dataType == Constants.DataTypes.Double)
                return reader.ReadDouble();

            if (dataType == Constants.DataTypes.Color)
                return reader.ReadColor();

            throw new ArgumentException();
        }

        public static decimal ReadDecimal(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            int i1 = reader.ReadInt32();
            int i2 = reader.ReadInt32();
            int i3 = reader.ReadInt32();
            int i4 = reader.ReadInt32();

            return new decimal(new int[] { i1, i2, i3, i4 });
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            byte a = reader.ReadByte();
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();

            return Color.FromArgb(a, r, g, b);
        }

        public static Guid ReadGuid(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            byte[] bytes = reader.ReadBytes(16);

            Guid guid = new Guid(bytes);

            return guid;
        }

    }
}