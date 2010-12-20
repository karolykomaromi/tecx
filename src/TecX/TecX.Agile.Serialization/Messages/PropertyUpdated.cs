using System;
using System.IO;
using System.Windows.Media;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public class PropertyUpdated : IBinarySerializable
    {
        public string PropertyName { get; set; }
        public Guid ArtefactId { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }

        public PropertyUpdated()
        {
            PropertyName = string.Empty;
            ArtefactId = Guid.Empty;
            OldValue = null;
            NewValue = null;
        }

        #region Implementation of IBinarySerializable

        public void WriteDataTo(BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            writer.Write(ArtefactId);
            writer.Write(PropertyName);

            if (OldValue == null)
                writer.Write(Constants.DataTypes.Null);

            if (NewValue == null)
                writer.Write(Constants.DataTypes.Null);

        }

        public void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            ArtefactId = reader.ReadGuid();
            PropertyName = reader.ReadString();

            OldValue = reader.ReadUnknown();

            byte dataType = reader.ReadByte();

            if(dataType == Constants.DataTypes.Null)
            {
                OldValue = null;
            }

            dataType = reader.ReadByte();

            if(dataType == Constants.DataTypes.Null)
            {
                NewValue = null;
            }
        }

        public int GetTypeId()
        {
            return Constants.TypeIds.PropertyUpdated;
        }

        #endregion
    }

    public static class BinaryReaderWriterExtensions
    {
        public static Guid ReadGuid(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            Guid guid = new Guid(reader.ReadBytes(16));

            return guid;
        }

        public static void Write(this BinaryWriter writer, Guid guid)
        {
            Guard.AssertNotNull(writer, "writer");

            writer.Write(guid.ToByteArray());
        }

        public static object ReadUnknown(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            byte dataType = reader.ReadByte();

            switch(dataType)
            {
                case Constants.DataTypes.Null:
                    return null;
                case Constants.DataTypes.String:
                    return reader.ReadString();
                case Constants.DataTypes.Color:
                    {
                        byte[] bytes = reader.ReadBytes(4);
                        return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
                    }
                case Constants.DataTypes.Decimal:
                    return reader.ReadDecimal();
                case Constants.DataTypes.Double:
                    return reader.ReadDouble();
                case Constants.DataTypes.Guid:
                    return reader.ReadGuid();
                default:
                    throw new ArgumentException();
            }
        }

        public static decimal ReadDecimal(this BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            int i1 = reader.ReadInt32();
            int i2 = reader.ReadInt32();
            int i3 = reader.ReadInt32();
            int i4 = reader.ReadInt32();

            return new decimal(new[]{i1, i2, i3, i4});
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
            
        }
    }

}
