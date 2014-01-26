namespace Infrastructure.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ListViewCell
    {
        [DataMember]
        public string PropertyName { get; set; }

        [DataMember]
        public string PropertyType { get; set; }

        [DataMember]
        public object Value { get; set; }
    }
}