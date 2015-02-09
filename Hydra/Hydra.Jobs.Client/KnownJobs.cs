namespace Hydra.Jobs.Client
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public enum KnownJobs
    {
        [EnumMember]
        Noop = 0,
    }
}
