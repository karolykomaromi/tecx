namespace TecX.EnumClasses.Test.TestObjects
{
    using System.Runtime.Serialization;

    [DataContract]
    public class WithFlags
    {
        [DataMember]
        public EnumWithFlags Flags { get; set; }
    }
}