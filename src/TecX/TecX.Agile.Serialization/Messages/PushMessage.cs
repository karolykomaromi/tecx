using System;
using System.IO;

using TecX.Common;

namespace TecX.Agile.Serialization.Messages
{
    public abstract class PushMessage : IBinarySerializable
    {
        public Guid SenderId { get; set; }

        protected PushMessage()
        {
            SenderId = Guid.Empty;
        }

        public virtual void WriteDataTo(BinaryWriter writer)
        {
            Guard.AssertNotNull(writer, "writer");

            writer.Write(SenderId);
        }

        public virtual void SetDataFrom(BinaryReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            SenderId = reader.ReadGuid();
        }

        public abstract int GetTypeId();
    }
}
