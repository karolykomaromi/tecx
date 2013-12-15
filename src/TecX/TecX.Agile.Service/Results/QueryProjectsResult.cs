namespace TecX.Agile.Service.Results
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof(Project))]
    public class QueryProjectsResult
    {
        [DataMember]
        public int TotalResultCount { get; set; }

        [DataMember]
        public Project[] Projects { get; set; }
    }
}