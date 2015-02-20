namespace Hydra.Jobs.Client
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public enum KnownJobs
    {
        [EnumMember]
        Nop = 0,

        [EnumMember]
        Heartbeat = 1,

        [EnumMember]
        BatchMail = 2,
    }
}
