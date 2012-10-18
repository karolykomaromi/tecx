namespace TecX.EnumClasses.Tests.TestObjects
{
    using System.Collections.Generic;
    using System.Linq;

    [PutDataContractSurrogateBehavior]
    public class SortingService : ISortingService
    {
        public IEnumerable<SerializeMe> Sort(IEnumerable<SerializeMe> itemsToSort, SortOrder sortOrder)
        {
            return itemsToSort.OrderBy(s => s.Text, sortOrder.Comparer);
        }
    }
}
