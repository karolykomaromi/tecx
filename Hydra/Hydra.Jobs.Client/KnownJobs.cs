namespace Hydra.Jobs.Client
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public enum KnownJobs
    {
        [EnumMember]
        NoOp = 0,

        [EnumMember]
        Heartbeat = 1,
    }
}
