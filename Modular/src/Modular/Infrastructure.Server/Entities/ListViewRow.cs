namespace Infrastructure.Entities
{
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Count={Cells.Length}")]
    public class ListViewRow
    {
        public ListViewRow()
        {
            this.Cells = new ListViewCell[0];
        }

        [DataMember]
        public ListViewCell[] Cells { get; set; }
    }
}