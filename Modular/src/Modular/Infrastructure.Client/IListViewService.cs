namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Infrastructure.Entities;

    [ServiceContract]
    [ContractClass(typeof(ListViewServiceContract))]
    public interface IListViewService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetListView(string listViewName, int pageNumber, int pageSize, AsyncCallback callback, object asyncState);

        ListView EndGetListView(IAsyncResult result);
    }

    [ContractClassFor(typeof(IListViewService))]
    internal abstract class ListViewServiceContract : IListViewService
    {
        public IAsyncResult BeginGetListView(string listViewName, int pageNumber, int pageSize, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(!string.IsNullOrEmpty(listViewName));
            Contract.Requires(pageNumber >= 0);
            Contract.Requires(pageSize >= 0);

            return null;
        }

        public ListView EndGetListView(IAsyncResult result)
        {
            return null;
        }
    }
}
