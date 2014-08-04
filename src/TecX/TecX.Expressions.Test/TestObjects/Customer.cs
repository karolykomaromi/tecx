namespace TecX.Expressions.Test.TestObjects
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }
    }
}