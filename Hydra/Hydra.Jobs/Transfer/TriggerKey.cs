namespace Hydra.Jobs.Transfer
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TriggerKey
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Group { get; set; }
    }
}