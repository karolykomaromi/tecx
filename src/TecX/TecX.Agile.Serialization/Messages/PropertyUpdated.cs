using System;
using System.IO;

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

            writer.Write(ArtefactId.ToByteArray());
            writer.Write(PropertyName);

            if (OldValue == null)
                writer.Write(Constants.DataTypes.Null);

            if (NewValue == null)
                writer.Write(Constants.DataTypes.Null);

        }

        public void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            ArtefactId = new Guid(reader.ReadBytes(16));
            PropertyName = reader.ReadString();

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
}
