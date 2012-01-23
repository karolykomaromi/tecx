namespace TecX.Agile.Phone.Service
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class StoryCard
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid IterationId { get; set; }
    }
}