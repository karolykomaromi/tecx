namespace Infrastructure.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ListViewRow
    {
        [DataMember]
        public ListViewCell[] Cells { get; set; }
    }
}