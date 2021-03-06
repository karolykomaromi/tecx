namespace TecX.Agile.Service.Results
{
    using System.Runtime.Serialization;

    [DataContract]
    public class QueryIterationsResult
    {
        [DataMember]
        public int TotalResultCount { get; set; }

        [DataMember]
        public Iteration[] Iterations { get; set; }
    }
}