using System;
using System.IO;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public class StoryCardMoved : IBinarySerializable
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

        public void WriteDataTo(BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            writer.Write(StoryCardId.ToByteArray());
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Angle);
        }

        public void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            StoryCardId = new Guid(reader.ReadBytes(16));
            X = reader.ReadDouble();
            Y = reader.ReadDouble();
            Angle = reader.ReadDouble();
        }

        public int GetTypeId()
        {
            return Constants.TypeIds.StoryCardMoved;
        }

        #endregion
    }
}
