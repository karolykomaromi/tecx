namespace TecX.EnumClasses.Tests.TestObjects
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SerializeMe
    {
        [DataMember]
        public SortOrder SortOrder { get; set; }
    }
}