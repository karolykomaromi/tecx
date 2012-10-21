namespace TecX.EnumClasses.Test.TestObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    [HideEnumerationClassesBehavior, ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SortingService : ISortingService
    {
        public IEnumerable<SerializeMe> Sort(IEnumerable<SerializeMe> itemsToSort, SortOrder sortOrder)
        {
            return itemsToSort.OrderBy(s => s.Text, sortOrder.Comparer);
        }
    }
}
