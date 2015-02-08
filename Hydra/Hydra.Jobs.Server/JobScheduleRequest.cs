namespace Hydra.Jobs.Server
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public abstract class JobScheduleRequest : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
    }
}