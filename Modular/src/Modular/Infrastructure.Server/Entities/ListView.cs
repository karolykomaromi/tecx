namespace Infrastructure.Entities
{
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Name={Name} Properties={Properties.Length} Rows={Rows.Length} Skip={Skip} Take={Take}")]
    public class ListView
    {
        public ListView()
        {
            this.Properties = new Property[0];
            this.Rows = new ListViewRow[0];
        }

        [DataMember]
        public Property[] Properties { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ListViewRow[] Rows { get; set; }

        [DataMember]
        public int Take { get; set; }

        [DataMember]
        public int Skip { get; set; }
    }
}