namespace Hydra.Jobs.Client
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class SimpleJobScheduleRequest : JobScheduleRequest
    {
        [DataMember]
        public bool RequestRecovery { get; set; }

        [DataMember]
        public bool StoryDurably { get; set; }

        [DataMember]
        public long IntervalInTicks { get; set; }

        [DataMember]
        public long StartAtInTicks { get; set; }

        [DataMember]
        public long StartAtOffsetInTicks { get; set; }
    }
}