namespace Infrastructure.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Property
    {
        [DataMember]
        public string PropertyName { get; set; }

        [DataMember]
        public string PropertyType { get; set; }
    }
}