using System;
using System.Runtime.Serialization;

namespace Search.Entities
{
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