namespace Hydra.Jobs.Server
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JobKey
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Group { get; set; }
    }
}