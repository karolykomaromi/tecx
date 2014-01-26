namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Infrastructure.Entities;

    [ServiceContract]
    [ContractClass(typeof(ListViewServiceContract))]
    public interface IListViewService
    {
        [OperationContract]
        ListView GetListView(string listViewName, int pageNumber, int pageSize);
    }

    [ContractClassFor(typeof(IListViewService))]
    internal abstract class ListViewServiceContract : IListViewService
    {
        public ListView GetListView(string listViewName, int pageNumber, int pageSize)
        {
            Contract.Requires(!string.IsNullOrEmpty(listViewName));
            Contract.Requires(pageNumber >= 0);
            Contract.Requires(pageSize >= 0);

            return null;
        }
    }
}