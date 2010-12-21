using System;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public class FieldHighlighted : PushMessage
    {
        public Guid ArtefactId { get; set; }
        public string FieldName { get; set; }

        public FieldHighlighted()
        {
            ArtefactId = Guid.Empty;
            FieldName = string.Empty;
        }

        #region Overrides of PushMessage

        public override void SetDataFrom(System.IO.BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            base.SetDataFrom(reader);

            ArtefactId = reader.ReadGuid();
            FieldName = reader.ReadString();
        }

        public override void WriteDataTo(System.IO.BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            base.WriteDataTo(writer);

            writer.Write(ArtefactId);
            writer.Write(FieldName);
        }

        public override int GetTypeId()
        {
            return Constants.MessageTypeIds.FieldHighlighted;
        }

        #endregion
    }
}
