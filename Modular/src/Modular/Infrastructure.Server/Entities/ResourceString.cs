namespace Infrastructure.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResourceString
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Culture { get; set; }
    }
}