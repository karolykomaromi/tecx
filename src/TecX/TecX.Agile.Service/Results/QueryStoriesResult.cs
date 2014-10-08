namespace TecX.Agile.Service.Results
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof(StoryCard))]
    public class QueryStoriesResult
    {
        [DataMember]
        public int TotalResultCount { get; set; }

        [DataMember]
        public StoryCard[] Stories { get; set; }
    }
}