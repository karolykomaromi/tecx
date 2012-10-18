namespace TecX.EnumClasses.Tests.TestObjects
{
    using System.Runtime.Serialization;

    [DataContract(Name = "SerializeMe")]
    public class SerializeMe2
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public SortOrderEnum SortOrder { get; set; }
    }
}