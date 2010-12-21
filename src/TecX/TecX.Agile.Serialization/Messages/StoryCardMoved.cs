using System;
using System.IO;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public class StoryCardMoved : PushMessage
    {
        public Guid StoryCardId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Angle { get; set; }

        public StoryCardMoved()
        {
            StoryCardId = Guid.Empty;
            X = 0.0;
            Y = 0.0;
            Angle = 0.0;
        }

        #region Implementation of IBinarySerializable

        public override void WriteDataTo(BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            base.WriteDataTo(writer);

            writer.Write(StoryCardId);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Angle);
        }

        public override void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            base.SetDataFrom(reader);

            StoryCardId = reader.ReadGuid();
            X = reader.ReadDouble();
            Y = reader.ReadDouble();
            Angle = reader.ReadDouble();
        }

        public override int GetTypeId()
        {
            return Constants.TypeIds.StoryCardMoved;
        }

        #endregion
    }
}
