namespace TecX.EnumClasses.Test.TestObjects
{
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface ISortingService
    {
        [OperationContract]
        IEnumerable<SerializeMe> Sort(IEnumerable<SerializeMe> itemsToSort, SortOrder sortOrder);
    }
}