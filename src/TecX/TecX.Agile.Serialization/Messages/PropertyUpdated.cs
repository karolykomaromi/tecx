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

            writer.Write(ArtefactId);
            writer.Write(PropertyName);

            writer.WriteUnkown(OldValue);
            writer.WriteUnkown(NewValue);
        }

        public void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            ArtefactId = reader.ReadGuid();
            PropertyName = reader.ReadString();

            OldValue = reader.ReadUnknown();

            NewValue = reader.ReadUnknown();
        }

        public int GetTypeId()
        {
            return Constants.TypeIds.PropertyUpdated;
        }

        #endregion
    }
}
