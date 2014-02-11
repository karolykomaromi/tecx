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
        IAsyncResult BeginGetListView(string listViewName, int skip, int take, AsyncCallback callback, object asyncState);

        ListView EndGetListView(IAsyncResult result);
    }

    [ContractClassFor(typeof(IListViewService))]
    internal abstract class ListViewServiceContract : IListViewService
    {
        public IAsyncResult BeginGetListView(string listViewName, int skip, int take, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(!string.IsNullOrEmpty(listViewName));
            Contract.Requires(skip >= 0);
            Contract.Requires(take >= 0);

            return null;
        }

        public ListView EndGetListView(IAsyncResult result)
        {
            return null;
        }
    }
}
