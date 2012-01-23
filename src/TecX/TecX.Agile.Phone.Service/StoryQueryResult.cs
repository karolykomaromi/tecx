namespace TecX.Agile.Phone.Service
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof(StoryCard))]
    public class StoryQueryResult
    {
        [DataMember]
        public int TotalResultCount { get; set; }

        [DataMember]
        public StoryCard[] Stories { get; set; }
    }
}