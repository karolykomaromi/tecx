namespace Infrastructure.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ListView
    {
        [DataMember]
        public Property[] Properties { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ListViewRow[] Rows { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int PageNumber { get; set; }
    }
}