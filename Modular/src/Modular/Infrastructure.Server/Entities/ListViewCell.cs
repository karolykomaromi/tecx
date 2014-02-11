namespace Infrastructure.Entities
{
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Name={PropertyName} Value={Value}")]
    public class ListViewCell
    {
        [DataMember]
        public string PropertyName { get; set; }

        [DataMember]
        public object Value { get; set; }
    }
}