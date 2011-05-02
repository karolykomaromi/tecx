using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;

namespace TecX.Agile.Remote
{
    public static class SerializationHelper
    {
        private static readonly DataContractSerializerCollection Serializers = new DataContractSerializerCollection();

        public static string Serialize(IDomainEvent @event)
        {
            Guard.AssertNotNull(@event, "@event");

            Type typeToSerialize = @event.GetType();

            DataContractSerializer serializer = Serializers[typeToSerialize];

            StringBuilder sb = new StringBuilder(1024);

            TextWriter tw = new StringWriter(sb);

            XmlWriter writer = XmlWriter.Create(tw);

            serializer.WriteObject(writer, @event);

            string message = sb.ToString();

            return message;
        }
    }
}