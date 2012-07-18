using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using TecX.Common;

namespace TecX.Agile.Remote
{
    public class DataContractSerializerCollection : IEnumerable<DataContractSerializer>
    {
        private readonly Dictionary<Type, DataContractSerializer> _serializers;

        public DataContractSerializer this[Type typeToSerialize]
        {
            get
            {
                DataContractSerializer serializer;
                if (_serializers.TryGetValue(typeToSerialize, out serializer))
                {
                    return serializer;
                }

                serializer = new DataContractSerializer(typeToSerialize);
                _serializers.Add(typeToSerialize, serializer);

                return serializer;
            }
        }

        public DataContractSerializerCollection()
            : this(Type.EmptyTypes)
        {
        }

        public DataContractSerializerCollection(IEnumerable<Type> typesToSerialize)
        {
            Guard.AssertNotNull(typesToSerialize, "typesToSerialize");

            _serializers = typesToSerialize.ToDictionary(t => t, t => new DataContractSerializer(t));
        }

        public IEnumerator<DataContractSerializer> GetEnumerator()
        {
            return _serializers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}