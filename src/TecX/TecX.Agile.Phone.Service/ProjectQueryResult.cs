namespace TecX.Agile.Phone.Service
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof(Project))]
    public class ProjectQueryResult
    {
        [DataMember]
        public int TotalResultCount { get; set; }

        [DataMember]
        public Project[] Projects { get; set; }
    }
}