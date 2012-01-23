namespace TecX.Agile.Phone.Service
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Iteration
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid ProjectId { get; set; }
    }
}