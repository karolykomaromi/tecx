namespace TecX.EnumClasses.Test.TestObjects
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SerializeMe
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public SortOrder SortOrder { get; set; }
    }
}