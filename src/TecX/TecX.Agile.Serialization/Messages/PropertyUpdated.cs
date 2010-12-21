using System;
using System.IO;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public class PropertyUpdated : PushMessage
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

        public override void WriteDataTo(BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            base.WriteDataTo(writer);

            writer.Write(ArtefactId);
            writer.Write(PropertyName);

            writer.WriteUnkown(OldValue);
            writer.WriteUnkown(NewValue);
        }

        public override void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            base.SetDataFrom(reader);

            ArtefactId = reader.ReadGuid();
            PropertyName = reader.ReadString();

            OldValue = reader.ReadUnknown();

            NewValue = reader.ReadUnknown();
        }

        public override int GetTypeId()
        {
            return Constants.TypeIds.PropertyUpdated;
        }

        #endregion
    }
}
