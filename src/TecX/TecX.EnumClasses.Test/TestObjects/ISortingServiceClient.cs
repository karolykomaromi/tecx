namespace TecX.EnumClasses.Tests.TestObjects
{
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Name = "ISortingService")]
    public interface ISortingServiceClient
    {
        [OperationContract]
        IEnumerable<SerializeMe2> Sort(IEnumerable<SerializeMe2> itemsToSort, SortOrderEnum sortOrder);
    }
}