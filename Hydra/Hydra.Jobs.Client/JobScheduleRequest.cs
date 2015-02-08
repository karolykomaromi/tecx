namespace Hydra.Jobs.Client
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class JobScheduleRequest : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
    }
}