namespace Search.Entities
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SearchResult
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FoundSearchTermIn { get; set; }

        [DataMember]
        public Uri Uri { get; set; }
    }
}