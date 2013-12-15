using System;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public class CaretMoved : PushMessage
    {
        public Guid ArtefactId { get; set; }
        public string FieldName { get; set; }
        public int CaretIndex { get; set; }

        public CaretMoved()
        {
            ArtefactId = Guid.Empty;
            FieldName = string.Empty;
            CaretIndex = 0;
        }

        #region Overrides of PushMessage

        public override void WriteDataTo(System.IO.BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            base.WriteDataTo(writer);

            writer.Write(ArtefactId);
            writer.Write(FieldName);
            writer.Write(CaretIndex);
        }

        public override void SetDataFrom(System.IO.BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            base.SetDataFrom(reader);

            ArtefactId = reader.ReadGuid();
            FieldName = reader.ReadString();
            CaretIndex = reader.ReadInt32();
        }

        public override int GetTypeId()
        {
            return Constants.MessageTypeIds.CaretMoved;
        }

        #endregion
    }
}
