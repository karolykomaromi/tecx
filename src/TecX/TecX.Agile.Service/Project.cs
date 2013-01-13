namespace TecX.Agile.Service
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Project
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }
}