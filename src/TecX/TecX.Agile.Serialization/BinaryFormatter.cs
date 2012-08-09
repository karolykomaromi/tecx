using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

using TecX.Common;

namespace TecX.Agile.Serialization
{
    /// <summary>
    /// Silverlight does not have a BinaryFormatter. If XML or JSON is too verbose or serialization is too slow we 
    /// need to use a <see cref="BinaryFormatter"/>
    /// </summary>
    public class BinaryFormatter
    {
        #region Constants

        private static class Constants
        {
            /// <summary>10000</summary>
            public const int DefaultStreamLength = 10000;
        }

        #endregion Constants

        #region Fields

        private readonly MemoryStream _writeStream;
        private readonly BinaryWriter _writer;

        private readonly MemoryStream _readStream;
        private readonly BinaryReader _reader;

        private readonly Dictionary<Type, int> _byType = new Dictionary<Type, int>();
        private readonly Dictionary<int, Type> _byId = new Dictionary<int, Type>();

        private readonly byte[] _lengthBuffer = new byte[4];
        private readonly byte[] _copyBuffer = new byte[2 * Constants.DefaultStreamLength];

        #endregion Fields

        #region c'tor

        public BinaryFormatter()
        {
            _writeStream = new MemoryStream(Constants.DefaultStreamLength);
            _writer = new BinaryWriter(_writeStream);

            _readStream = new MemoryStream(Constants.DefaultStreamLength);
            _reader = new BinaryReader(_readStream);
        }

        #endregion c'tor

        #region Public Methods

        /// <summary>
        /// Registers the specified type id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeId">The type id.</param>
        public void Register<T>(int typeId)
            where T : IBinarySerializable, new()
        {
            _byId.Add(typeId, typeof(T));
            _byType.Add(typeof(T), typeId);
        }

        /// <summary>
        /// Deserializes an object from the specified byte array
        /// </summary>
        /// <param name="serializedData">The byte array with serialized object data.</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize(byte[] serializedData)
        {
            Guard.AssertNotNull(serializedData, "serializedData");

            using (var stream = new MemoryStream(serializedData))
            {
                return Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserializes an object from the stream.
        /// </summary>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize(Stream serializationStream)
        {
            Guard.AssertNotNull(serializationStream, "serializationStream");

            //read the preamble from the stream that tells us the number of bytes
            //that were written into the stream
            int bytesRead = serializationStream.Read(_lengthBuffer, 0, 4);
            if (bytesRead != 4)
                throw new SerializationException("Could not read length of transfered data from the stream.");

            IntToBytes length = new IntToBytes(_lengthBuffer[0], _lengthBuffer[1], _lengthBuffer[2], _lengthBuffer[3]);

            //TODO weberse 2010-12.20 make this support partial reads from stream
            bytesRead = serializationStream.Read(_copyBuffer, 0, length.Int32);
            if (bytesRead != length.Int32)
                throw new SerializationException("Could not read " + length + " bytes from the stream.");

            _readStream.Seek(0, SeekOrigin.Begin);
            _readStream.Write(_copyBuffer, 0, length.Int32);
            _readStream.Seek(0, SeekOrigin.Begin);
            int typeid = _reader.ReadInt32();

            Type type;
            if (!_byId.TryGetValue(typeid, out type))
                throw new SerializationException("TypeId " + typeid + " is not a registered type id");

            //TODO weberse 2010-12-20 should i migrate this to unity?
            IBinarySerializable deserialized = (IBinarySerializable)Activator.CreateInstance(type);

            deserialized.SetDataFrom(_reader);

            if (_readStream.Position != length.Int32)
                throw new SerializationException("Object of type " + type + " did not read its entire buffer during "
                                                 + "deserialization. This is most likely an inbalance between"
                                                 + " the writes and the reads of the object.");

            return deserialized;
        }

        /// <summary>
        /// Serializes the specified object graph
        /// </summary>
        /// <param name="serializationStream">The serialization stream.</param>
        /// <param name="graph">The object graph.</param>
        public void Serialize(Stream serializationStream, object graph)
        {
            Guard.AssertNotNull(serializationStream, "serializationStream");
            Guard.AssertNotNull(graph, "graph");

            var serializable = (IBinarySerializable) graph;

            int key;
            if (!_byType.TryGetValue(graph.GetType(), out key))
                throw new SerializationException(graph.GetType() + " has not been registered with the serializer");

            _writeStream.Seek(0L, SeekOrigin.Begin);
            _writer.Write(key);
            serializable.WriteDataTo(_writer);

            IntToBytes numberOfBytesWritten = new IntToBytes((int)_writeStream.Position);

            serializationStream.WriteByte(numberOfBytesWritten.Byte0);
            serializationStream.WriteByte(numberOfBytesWritten.Byte1);
            serializationStream.WriteByte(numberOfBytesWritten.Byte2);
            serializationStream.WriteByte(numberOfBytesWritten.Byte3);

            serializationStream.Write(_writeStream.GetBuffer(), 0, (int)_writeStream.Position);
        }

        /// <summary>
        /// Serializes the specified object graph.
        /// </summary>
        /// <param name="graph">The object graph.</param>
        /// <returns>The data of the binary serialized object <paramref name="graph"/></returns>
        public byte[] Serialize(object graph)
        {
            Guard.AssertNotNull(graph, "graph");

            using (var stream = new MemoryStream())
            {
                Serialize(stream, graph);

                byte[] serialized = stream.ToArray();

                return serialized;
            }
        }

        #endregion Public Methods
    }
}
