namespace Infrastructure.Entities
{
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Name={PropertyName} Type={PropertyType} Resx={ResourceKey}")]
    public class Property
    {
        [DataMember]
        public string PropertyName { get; set; }

        [DataMember]
        public string PropertyType { get; set; }

        [DataMember]
        public string ResourceKey { get; set; }
    }
}